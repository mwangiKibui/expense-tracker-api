using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class BudgetGoalCategoryService : IBudgetGoalCategoryService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public BudgetGoalCategoryService(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<BudgetGoalCategory> GetBudgetGoalCategoriesQueryable(){
            var query =  _dbContext.BudgetGoalCategories.Where(sts => sts.IsDeleted == false);
            return query;
        }

        private IQueryable<BudgetGoalCategory> FilterBudgetGoalCategories(IQueryable<BudgetGoalCategory> query, CustomQueryDto customQueryDto){
            var searchIsEmpty = customQueryDto.SearchValue is null || string.IsNullOrEmpty(customQueryDto.SearchValue);
            if(!searchIsEmpty){
                query = query.Where(budgtGoalCat => budgtGoalCat.Name.ToLower().Contains(customQueryDto.SearchValue!.ToLower()));
            }
            return query;
        }

        public async Task<ListResponseBudgetGoalCategoryDto> AddBudgetGoalCategory(AuthenticatedUser? authenticatedUser,List<AddBudgetGoalCategoryDto> addBudgetGoalCategoryDtos)
        {
            ListResponseBudgetGoalCategoryDto response = new ListResponseBudgetGoalCategoryDto();
            List<BudgetGoalCategory> addedBudgetGoalCategories = new List<BudgetGoalCategory>();
            foreach(var addBudgetGoalCategoryDto in addBudgetGoalCategoryDtos)
            {
                BudgetGoalCategory? budgetGoalCategoryExists = await _dbContext.BudgetGoalCategories.Where(goal => goal.Name.ToLower() == addBudgetGoalCategoryDto.Name.ToLower()).FirstOrDefaultAsync();
                if(budgetGoalCategoryExists != null){
                    addedBudgetGoalCategories.Add(budgetGoalCategoryExists);
                    continue;
                }else{
                    BudgetGoalCategory newBugetGoalCategory = new BudgetGoalCategory{
                        Name = addBudgetGoalCategoryDto.Name,
                        Description = addBudgetGoalCategoryDto.Description,
                        CreatedBy = authenticatedUser!.UserId
                    };
                    await _dbContext.AddAsync(newBugetGoalCategory);
                    addedBudgetGoalCategories.Add(newBugetGoalCategory);
                }
            } 
            await _dbContext.SaveChangesAsync();
            response.Success = true;
            response.Message = "Budget goal categories added successfully";
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Results = _mapper.Map<List<BudgetGoalCategoryDto>>(addedBudgetGoalCategories);
            return response;
        }

        public async Task<ListResponseBudgetGoalCategoryDto> GetBudgetGoalCategory(CustomQueryDto customQueryDto)
        {
            ListResponseBudgetGoalCategoryDto response = new ListResponseBudgetGoalCategoryDto();
            var query = GetBudgetGoalCategoriesQueryable();
            query = FilterBudgetGoalCategories(query,customQueryDto);
            if(customQueryDto.SortOrder == "DESC"){
                query = query.OrderByDescending(budgetGoalCat => budgetGoalCat.Name);
            }else{
                query = query.OrderBy(budgetGoalCat => budgetGoalCat.Name);
            }
            var totalCount = await query.CountAsync();
            var pageSize = customQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = customQueryDto.PageNumber > 0 ? customQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<BudgetGoalCategory> results = await query.ToListAsync();
            response.Success = true;
            response.Message = "Budget Goal Categories fetched successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Results = _mapper.Map<List<BudgetGoalCategoryDto>>(results);
            response.CurrentPage = currentPage;
            response.TotalCount = totalCount;
            response.TotalPages = totalPages;
            return response;
        }

        public async Task<ResponseBudgetGoalCategoryDto> UpdateBudgetGoalCategory(Guid budgetGoalCategoryId,UpdateBudgetGoalCategoryDto updateBudgetGoalCategoryDto)
        {
            ResponseBudgetGoalCategoryDto response = new ResponseBudgetGoalCategoryDto();
            BudgetGoalCategory? budgetGoalCategoryExists = await _dbContext.BudgetGoalCategories.Where(budgtGoalCat => budgtGoalCat.BudgetGoalCategoryId == budgetGoalCategoryId).FirstOrDefaultAsync();
            if(budgetGoalCategoryExists == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "No such budget goal category exists";
            }else{
                budgetGoalCategoryExists.Name = updateBudgetGoalCategoryDto.Name;
                budgetGoalCategoryExists.Description = updateBudgetGoalCategoryDto.Description;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<BudgetGoalCategoryDto>(budgetGoalCategoryExists);
            }
            return response;
        }

        public async Task<ResponseBudgetGoalCategoryDto> DeleteBudgetGoalCategory(AuthenticatedUser? authenticatedUser,Guid budgetGoalCategoryId)
        {
            ResponseBudgetGoalCategoryDto response = new ResponseBudgetGoalCategoryDto();
            BudgetGoalCategory? budgetGoalCategoryExists = await _dbContext.BudgetGoalCategories.Where(budgtGoalCat => budgtGoalCat.IsDeleted == false && budgtGoalCat.BudgetGoalCategoryId == budgetGoalCategoryId).FirstOrDefaultAsync();
            if(budgetGoalCategoryExists == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "No such budget goal category exists";
            }else{
                budgetGoalCategoryExists.DeletedAt = DateTime.UtcNow;
                budgetGoalCategoryExists.DeletedBy = authenticatedUser!.UserId;
                budgetGoalCategoryExists.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<BudgetGoalCategoryDto>(budgetGoalCategoryExists);
            }
            return response;
        }
    }
}