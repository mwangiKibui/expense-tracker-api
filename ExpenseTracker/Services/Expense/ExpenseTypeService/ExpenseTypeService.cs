using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ExpenseTypeService(DataContext dbContext,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private static readonly Dictionary<dynamic, IOrderBy> OrderBys = new()
        {
            { "name", new OrderBy<ExpenseType, string>(s => s.Name) },
        };

        private IQueryable<ExpenseType> GetExpenseTypesQueryable(){
            var query =  _dbContext.ExpenseTypes.Where(sts => sts.IsDeleted == false);
            return query;
        }

        private IQueryable<ExpenseType> FilterExpenseTypes(IQueryable<ExpenseType> query, ExpenseTypeQueryDto expenseTypeQueryDto){
            var searchIsEmpty = expenseTypeQueryDto.SearchValue is null || string.IsNullOrEmpty(expenseTypeQueryDto.SearchValue);
            if(!searchIsEmpty){
                query = query.Where(sts => sts.Name.ToLower().Contains(expenseTypeQueryDto.SearchValue!.ToLower()));
            }
            return query;
        }

        public async Task<ResponseExpenseTypeDto> AddExpenseType(AuthenticatedUser? authenticatedUser,AddExpenseTypeDto addExpenseTypeDto){
            ResponseExpenseTypeDto response = new ResponseExpenseTypeDto();
            if(authenticatedUser is null){
                response.Success = false;
                response.Message = "Unauthenticated user";
                response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }else{
                var duplicateExists = await _dbContext.ExpenseTypes.Where(exp => exp.Name.ToLower() == addExpenseTypeDto.Name.ToLower()).FirstOrDefaultAsync();
                if(duplicateExists is not null){
                    if(duplicateExists.IsDeleted){ // reactivate the expense type.
                        duplicateExists.IsDeleted = false;
                        duplicateExists.DeletedAt = null;
                        duplicateExists.DeletedBy = null;
                        await _dbContext.SaveChangesAsync();
                        response.Success = true;
                        response.Message = "Expense Category restored successfully";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Data = _mapper.Map<ExpenseTypeDto>(duplicateExists);
                    }else{
                        response.Success = false;
                        response.Message = "Expense Type already exists";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        response.Data = _mapper.Map<ExpenseTypeDto>(duplicateExists);
                    }
                }else{
                    var newExpenseType = new ExpenseType{
                        Name = addExpenseTypeDto.Name,
                        Description = addExpenseTypeDto.Description,
                        SystemDefault = addExpenseTypeDto.SystemDefault,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = authenticatedUser.UserId
                    };
                    await _dbContext.AddAsync(newExpenseType);
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Expense Type Created Successfully";
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    response.Data = _mapper.Map<ExpenseTypeDto>(newExpenseType);
                }
            }
            return response;
        }

        public async Task<ListResponseExpenseTypeDto> GetExpenseType(ExpenseTypeQueryDto expenseTypeQueryDto){
            // check if there is a filter.
            var query = GetExpenseTypesQueryable();
            query = FilterExpenseTypes(query,expenseTypeQueryDto);
            if(expenseTypeQueryDto.SortOrder == "DESC"){
                query = query.OrderByDescending(expense => expense.Name);
            }else{
                query = query.OrderBy(expense => expense.Name);
            }
            var totalCount = await query.CountAsync();
            var pageSize = expenseTypeQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = expenseTypeQueryDto.PageNumber > 0 ? expenseTypeQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<ExpenseType> results = await query.ToListAsync();  
            ListResponseExpenseTypeDto response = new ListResponseExpenseTypeDto{
                Success = true,
                Message = "Fetched Expense Types Successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Results = _mapper.Map<List<ExpenseTypeDto>>(results),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };
            return response;
        } 

        // update an expense type.
        public async Task<ResponseExpenseTypeDto> UpdateExpenseType(Guid expenseTypeId,UpdateExpenseTypeDto updateExpenseTypeDto){
            // confirm the expense type exists.
            ResponseExpenseTypeDto response = new ResponseExpenseTypeDto();
            var query = GetExpenseTypesQueryable();
            var expenseTypeExists = await query.Where(expenseType => expenseType.ExpenseTypeId == expenseTypeId).FirstOrDefaultAsync();
            if(expenseTypeExists is null){
                response.Success = false;
                response.Message = "No such expense type exists";
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
            }else{
                expenseTypeExists.Name = updateExpenseTypeDto.Name;
                if(updateExpenseTypeDto.Description != null){
                    expenseTypeExists.Description = updateExpenseTypeDto.Description;
                }
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Expense Type Updated Successfullly";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<ExpenseTypeDto>(expenseTypeExists);
            }
            return response;
        }

        // delete an expense type.
        public async Task<ResponseExpenseTypeDto> DeleteExpenseType(AuthenticatedUser? authenticatedUser,Guid expenseTypeId){
            // confirm the expense type exists.
            ResponseExpenseTypeDto response = new ResponseExpenseTypeDto();
            var query = GetExpenseTypesQueryable();
            var expenseTypeExists = await query.Where(expenseType => expenseType.ExpenseTypeId == expenseTypeId).FirstOrDefaultAsync();
            if(expenseTypeExists is null){
                response.Success = false;
                response.Message = "No such expense type exists";
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
            }else{
                expenseTypeExists.IsDeleted = true;
                expenseTypeExists.DeletedBy = authenticatedUser!.UserId;
                expenseTypeExists.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Expense Type deleted Successfullly";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<ExpenseTypeDto>(expenseTypeExists);
            }
            return response;
        }


    }
}