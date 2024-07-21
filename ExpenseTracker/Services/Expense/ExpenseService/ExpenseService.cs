using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public ExpenseService(DataContext dbContext,IMapper mapper){
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<Expense> GetExpensesQueryable(){
            var query =  _dbContext.Expenses.Where(sts => sts.IsDeleted == false);
            return query;
        }

        private async Task<ExpenseType> validateExpenseType(Guid expenseTypeId){
            ExpenseType? result = await _dbContext.ExpenseTypes.Where(expType => expType.ExpenseTypeId == expenseTypeId).FirstOrDefaultAsync();
            return result!;
        }

        private async Task<Expense> validateExpense(Guid expenseId){
            Expense? result = await _dbContext.Expenses.Where(exp => exp.ExpenseID == expenseId).FirstOrDefaultAsync();
            return result!;
        }

        private async Task<Expense> checkDuplicate(string name,int expenseTypeId,int userId){
            var result = await _dbContext.Expenses.Where(
                exp => exp.Name.ToLower() == name.ToLower() && 
                exp.ExpenseTypeId == expenseTypeId &&
                exp.UserId == userId
            ).FirstOrDefaultAsync();
            return result!;
        }

        private IQueryable<Expense> FilterExpenses(IQueryable<Expense> query, ExpenseQueryDto expenseQueryDto){
            var searchIsEmpty = expenseQueryDto.SearchValue is null || string.IsNullOrEmpty(expenseQueryDto.SearchValue);
            var userIdIsEmpty = expenseQueryDto.UserId is null || string.IsNullOrEmpty(expenseQueryDto.UserId);
            if(!searchIsEmpty){
                query = query.Where(exp => exp.Name.ToLower().Contains(expenseQueryDto.SearchValue!.ToLower()));
            }
            if(!userIdIsEmpty){
                query = query.Where(exp => exp.User!.UserID.ToString() == expenseQueryDto.UserId);
            }
            return query;
        }

        public async Task<ResponseExpenseDto> AddExpense (AuthenticatedUser? authenticatedUser, AddExpenseDto addExpenseDto){
            ExpenseType? expenseType = await validateExpenseType(addExpenseDto.ExpenseTypeId);
            ResponseExpenseDto response = new ResponseExpenseDto();

            if(expenseType is null){
                response.Success = false;
                response.StatusCode= System.Net.HttpStatusCode.BadRequest;
                response.Message =  "Expense Type not found";
            }else{
                Expense? duplicateExists = await checkDuplicate(addExpenseDto.Name,expenseType.Id,authenticatedUser!.Id);
                if(duplicateExists is not null){
                    response.Success = false;
                    response.Message = "Expense exists with name "+addExpenseDto.Name+" of category "+expenseType.Name;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }else{
                    var newExpense = new Expense {
                        ExpenseTypeId = expenseType.Id,
                        UserId = authenticatedUser!.Id,
                        Name = addExpenseDto.Name,
                        isRecurring = addExpenseDto.isRecurring,
                        reminderEnabled = addExpenseDto.reminderEnabled,
                        CreatedBy = authenticatedUser!.UserId,
                        CreatedAt = DateTime.UtcNow,
                    };
                    if(addExpenseDto.reminderEnabled){
                        newExpense.NatureOfRecurrence = addExpenseDto.natureOfRecurrence;
                        newExpense.ReminderStartDate = addExpenseDto.reminderStartDate;
                    }
                    await _dbContext.AddAsync(newExpense);
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    response.Message = "Expense added successfully";
                    response.Data = _mapper.Map<ExpenseDto>(newExpense);
                }
            }
            return response;
        }

        public async Task<ResponseExpenseDto> GetSingleExpense (Guid expenseId){
            ResponseExpenseDto response = new ResponseExpenseDto();
            Expense? expense = await _dbContext.Expenses.Where(exp => exp.ExpenseID == expenseId)
                    .Include(expense => expense.ExpenseType)
                    .Include(expense => expense.User)
                    .FirstOrDefaultAsync();
            if(expense is null){
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Expense not found";
            }else{
                response.Success = true;
                response.Message =  "Expense fetched successfully";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<ExpenseDto>(expense);
            }
            return response;
        }

        public async Task<ListResponseExpenseDto> GetExpenses(ExpenseQueryDto expenseQueryDto){
            ListResponseExpenseDto response = new ListResponseExpenseDto();
            var query = GetExpensesQueryable();
            query = FilterExpenses(query,expenseQueryDto);
            if(expenseQueryDto.SortOrder == "DESC"){
                query = query.OrderByDescending(expense => expense.Name);
            }else{
                query = query.OrderBy(expense => expense.Name);
            }
            var totalCount = await query.CountAsync();
            var pageSize = expenseQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = expenseQueryDto.PageNumber > 0 ? expenseQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<Expense> results = await query.ToListAsync();
            response.Success = true;
            response.Message = "Expenses fetched successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Results = _mapper.Map<List<ExpenseDto>>(results);
            response.CurrentPage = currentPage;
            response.TotalCount = totalCount;
            response.TotalPages = totalPages;
            return response;
        }

        public async Task<ResponseExpenseDto> UpdateExpense(Guid expenseId,UpdateExpenseDto updateExpenseDto){
            ResponseExpenseDto response = new ResponseExpenseDto();
            Expense? expense = await validateExpense(expenseId);
            if(expense is null){
                response.Success = false;
                response.Message = "Expense not found";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }else{
                if(updateExpenseDto.ExpenseTypeId is not null && updateExpenseDto.ExpenseTypeId > 0){
                    expense.ExpenseTypeId = (int)updateExpenseDto.ExpenseTypeId;
                }
                if(updateExpenseDto.isRecurring is not null){
                    expense.isRecurring = (bool)updateExpenseDto.isRecurring;
                }
                if(updateExpenseDto.reminderEnabled is not null){
                    expense.reminderEnabled = (bool)updateExpenseDto.reminderEnabled;
                }
                if(updateExpenseDto.natureOfRecurrence is not null){
                    expense.NatureOfRecurrence = updateExpenseDto.natureOfRecurrence;
                }
                if(updateExpenseDto.reminderStartDate is not null){
                    expense.ReminderStartDate = updateExpenseDto.reminderStartDate;
                }
                if(updateExpenseDto.Name is not null){
                    expense.Name = (string)updateExpenseDto.Name;
                }
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Expense updated successfully";
                response.Data = _mapper.Map<ExpenseDto>(expense);
            }
            return response;
        }

        public async Task<ResponseExpenseDto> DeleteExpense(AuthenticatedUser? authenticatedUser,Guid expenseId){
            ResponseExpenseDto response = new ResponseExpenseDto();
            Expense? expense = await validateExpense(expenseId);
            if(expense is null){
                response.Success = false;
                response.Message = "Expense not found";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }else{
                expense.IsDeleted = true;
                expense.DeletedAt = DateTime.UtcNow;
                expense.DeletedBy = authenticatedUser!.UserId;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Expense deleted successfully";
                response.Data = _mapper.Map<ExpenseDto>(expense);
            }
            return response;
        }
    }
}