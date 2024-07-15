using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public IncomeService(DataContext dbContext,IMapper mapper){
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<Income> GetIncomesQueryable(){
            var query =  _dbContext.Incomes.Where(sts => sts.isDeleted == false);
            return query;
        }

        private async Task<IncomeType> validateIncomeType(Guid incomeTypeId){
            IncomeType? result = await _dbContext.IncomeTypes.Where(expType => expType.IncomeTypeID == incomeTypeId).FirstOrDefaultAsync();
            return result!;
        }

        private async Task<Income> validateIncome(Guid incomeId){
            Income? result = await _dbContext.Incomes.Where(exp => exp.IncomeID == incomeId).FirstOrDefaultAsync();
            return result!;
        }

        private async Task<Income> checkDuplicate(string name,int incomeTypeId){
            var result = await _dbContext.Incomes.Where(exp => exp.Name.ToLower() == name.ToLower() && exp.IncomeTypeId == incomeTypeId).FirstOrDefaultAsync();
            return result!;
        }

        private IQueryable<Income> FilterIncomes(IQueryable<Income> query, IncomeQueryDto incomeQueryDto){
            var searchIsEmpty = incomeQueryDto.SearchValue is null || string.IsNullOrEmpty(incomeQueryDto.SearchValue);
            var userIdIsEmpty = incomeQueryDto.UserId is null || string.IsNullOrEmpty(incomeQueryDto.UserId);
            if(!searchIsEmpty){
                query = query.Where(exp => exp.Name.ToLower().Contains(incomeQueryDto.SearchValue!.ToLower()));
            }
            if(!userIdIsEmpty){
                query = query.Where(exp => exp.User!.UserID.ToString() == incomeQueryDto.UserId);
            }
            return query;
        }

        public async Task<ResponseIncomeDto> AddIncome (AuthenticatedUser? authenticatedUser, AddIncomeDto addIncomeDto){
            IncomeType? incomeType = await validateIncomeType(addIncomeDto.IncomeTypeId);
            ResponseIncomeDto response = new ResponseIncomeDto();

            if(incomeType is null){
                response.Success = false;
                response.StatusCode= System.Net.HttpStatusCode.BadRequest;
                response.Message =  "Income Type not found";
            }else{
                Income? duplicateExists = await checkDuplicate(addIncomeDto.Name,incomeType.Id);
                if(duplicateExists is not null){
                    response.Success = false;
                    response.Message = "Income exists with name "+addIncomeDto.Name+" of category "+incomeType.Name;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Data = _mapper.Map<IncomeDto>(duplicateExists);
                }else{
                    var newIncome = new Income {
                        IncomeTypeId = incomeType.Id,
                        UserId = authenticatedUser!.Id,
                        Name = addIncomeDto.Name,
                        isRecurring = addIncomeDto.IsRecurring,
                        reminderEnabled = addIncomeDto.ReminderEnabled,
                        CreatedBy = authenticatedUser!.UserId,
                        CreatedAt = DateTime.UtcNow,
                    };
                    if((bool)addIncomeDto.ReminderEnabled!){
                        newIncome.NatureOfRecurrence = addIncomeDto.NatureOfRecurrence;
                        newIncome.ReminderStartDate = addIncomeDto.ReminderStartDate;
                    }
                    await _dbContext.AddAsync(newIncome);
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    response.Message = "Income added successfully";
                    response.Data = _mapper.Map<IncomeDto>(newIncome);
                }
            }
            return response;
        }

        public async Task<ResponseIncomeDto> GetSingleIncome (Guid incomeId){
            ResponseIncomeDto response = new ResponseIncomeDto();
            Income? income = await _dbContext.Incomes.Where(exp => exp.IncomeID == incomeId)
                    .Include(income => income.IncomeType)
                    .Include(income => income.User)
                    .FirstOrDefaultAsync();
            if(income is null){
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Income not found";
            }else{
                response.Success = true;
                response.Message =  "Income fetched successfully";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<IncomeDto>(income);
            }
            return response;
        }

        public async Task<ListResponseIncomeDto> GetIncomes(IncomeQueryDto incomeQueryDto){
            ListResponseIncomeDto response = new ListResponseIncomeDto();
            var query = GetIncomesQueryable();
            query = FilterIncomes(query,incomeQueryDto);
            if(incomeQueryDto.SortOrder == "DESC"){
                query = query.OrderByDescending(income => income.Name);
            }else{
                query = query.OrderBy(income => income.Name);
            }
            var totalCount = await query.CountAsync();
            var pageSize = incomeQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = incomeQueryDto.PageNumber > 0 ? incomeQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<Income> results = await query.ToListAsync();
            response.Success = true;
            response.Message = "Incomes fetched successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Results = _mapper.Map<List<IncomeDto>>(results);
            response.CurrentPage = currentPage;
            response.TotalCount = totalCount;
            response.TotalPages = totalPages;
            return response;
        }

        public async Task<ResponseIncomeDto> UpdateIncome(Guid incomeId,UpdateIncomeDto updateIncomeDto){
            ResponseIncomeDto response = new ResponseIncomeDto();
            Income? income = await validateIncome(incomeId);
            if(income is null){
                response.Success = false;
                response.Message = "Income not found";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }else{
                if(updateIncomeDto.IncomeTypeId is not null && updateIncomeDto.IncomeTypeId > 0){
                    income.IncomeTypeId = (int)updateIncomeDto.IncomeTypeId;
                }
                if(updateIncomeDto.IsRecurring is not null){
                    income.isRecurring = (bool)updateIncomeDto.IsRecurring;
                }
                if(updateIncomeDto.ReminderEnabled is not null){
                    income.reminderEnabled = (bool)updateIncomeDto.ReminderEnabled;
                }
                if(updateIncomeDto.NatureOfRecurrence is not null){
                    income.NatureOfRecurrence = updateIncomeDto.NatureOfRecurrence;
                }
                if(updateIncomeDto.ReminderStartDate is not null){
                    income.ReminderStartDate = updateIncomeDto.ReminderStartDate;
                }
                if(updateIncomeDto.Name is not null){
                    income.Name = (string)updateIncomeDto.Name;
                }
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Income updated successfully";
                response.Data = _mapper.Map<IncomeDto>(income);
            }
            return response;
        }

        public async Task<ResponseIncomeDto> DeleteIncome(AuthenticatedUser? authenticatedUser,Guid incomeId){
            ResponseIncomeDto response = new ResponseIncomeDto();
            Income? income = await validateIncome(incomeId);
            if(income is null){
                response.Success = false;
                response.Message = "Income not found";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }else{
                income.isDeleted = true;
                income.DeletedAt = DateTime.UtcNow;
                income.DeletedBy = authenticatedUser!.UserId;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Income deleted successfully";
                response.Data = _mapper.Map<IncomeDto>(income);
            }
            return response;
        }
    }
}