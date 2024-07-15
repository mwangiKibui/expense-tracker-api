using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class IncomeTypeService : IIncomeTypeService
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public IncomeTypeService(DataContext dbContext,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private IQueryable<IncomeType> GetIncomeTypesQueryable(){
            var query =  _dbContext.IncomeTypes.Where(sts => sts.isDeleted == false);
            return query;
        }

        private IQueryable<IncomeType> FilterIncomeTypes(IQueryable<IncomeType> query, IncomeTypeQueryDto incomeTypeQueryDto){
            var searchIsEmpty = incomeTypeQueryDto.SearchValue is null || string.IsNullOrEmpty(incomeTypeQueryDto.SearchValue);
            if(!searchIsEmpty){
                query = query.Where(sts => sts.Name.ToLower().Contains(incomeTypeQueryDto.SearchValue!.ToLower()));
            }
            return query;
        }

        public async Task<ResponseIncomeTypeDto> AddIncomeType(AuthenticatedUser? authenticatedUser,AddIncomeTypeDto addIncomeTypeDto){
            ResponseIncomeTypeDto response = new ResponseIncomeTypeDto();
            if(authenticatedUser is null){
                response.Success = false;
                response.Message = "Unauthenticated user";
                response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }else{
                var duplicateExists = await _dbContext.IncomeTypes.Where(exp => exp.Name.ToLower() == addIncomeTypeDto.Name.ToLower()).FirstOrDefaultAsync();
                if(duplicateExists is not null){
                    if(duplicateExists.isDeleted){ // reactivate the expense type.
                        duplicateExists.isDeleted = false;
                        duplicateExists.DeletedAt = null;
                        duplicateExists.DeletedBy = null;
                        await _dbContext.SaveChangesAsync();
                        response.Success = true;
                        response.Message = "Income Category restored successfully";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Data = _mapper.Map<IncomeTypeDto>(duplicateExists);
                    }else{
                        response.Success = false;
                        response.Message = "Income Category already exists";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        response.Data = _mapper.Map<IncomeTypeDto>(duplicateExists);
                    }
                }else{
                    var newIncomeType = new IncomeType{
                        Name = addIncomeTypeDto.Name,
                        Description = addIncomeTypeDto.Description,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = authenticatedUser.UserId
                    };
                    await _dbContext.AddAsync(newIncomeType);
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Income Category Created Successfully";
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    response.Data = _mapper.Map<IncomeTypeDto>(newIncomeType);
                }
            }
            return response;
        }

        public async Task<ListResponseIncomeTypeDto> GetIncomeType(IncomeTypeQueryDto incomeTypeQueryDto){
            // check if there is a filter.
            var query = GetIncomeTypesQueryable();
            query = FilterIncomeTypes(query,incomeTypeQueryDto);
            if(incomeTypeQueryDto.SortOrder == "DESC"){
                query = query.OrderByDescending(expense => expense.Name);
            }else{
                query = query.OrderBy(expense => expense.Name);
            }
            var totalCount = await query.CountAsync();
            var pageSize = incomeTypeQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = incomeTypeQueryDto.PageNumber > 0 ? incomeTypeQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<IncomeType> results = await query.ToListAsync();  
            ListResponseIncomeTypeDto response = new ListResponseIncomeTypeDto{
                Success = true,
                Message = "Fetched Income Types Successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Results = _mapper.Map<List<IncomeTypeDto>>(results),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };
            return response;
        } 

        // update an expense type.
        public async Task<ResponseIncomeTypeDto> UpdateIncomeType(Guid incomeTypeId,UpdateIncomeTypeDto updateIncomeTypeDto){
            // confirm the expense type exists.
            ResponseIncomeTypeDto response = new ResponseIncomeTypeDto();
            var query = GetIncomeTypesQueryable();
            var incomeTypeExists = await query.Where(incomeType => incomeType.IncomeTypeID == incomeTypeId).FirstOrDefaultAsync();
            if(incomeTypeExists is null){
                response.Success = false;
                response.Message = "No such income type exists";
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
            }else{
                incomeTypeExists.Name = updateIncomeTypeDto.Name;
                if(updateIncomeTypeDto.Description != null){
                    incomeTypeExists.Description = updateIncomeTypeDto.Description;
                }
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Income Type Updated Successfullly";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<IncomeTypeDto>(incomeTypeExists);
            }
            return response;
        }

        // delete an expense type.
        public async Task<ResponseIncomeTypeDto> DeleteIncomeType(AuthenticatedUser? authenticatedUser,Guid incomeTypeId){
            // confirm the expense type exists.
            ResponseIncomeTypeDto response = new ResponseIncomeTypeDto();
            var query = GetIncomeTypesQueryable();
            var incomeTypeExists = await query.Where(incomeType => incomeType.IncomeTypeID == incomeTypeId).FirstOrDefaultAsync();
            if(incomeTypeExists is null){
                response.Success = false;
                response.Message = "No such income type exists";
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
            }else{
                incomeTypeExists.isDeleted = true;
                incomeTypeExists.DeletedBy = authenticatedUser!.UserId;
                incomeTypeExists.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Expense Type restored Successfullly";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<IncomeTypeDto>(incomeTypeExists);
            }
            return response;
        }


    }
}