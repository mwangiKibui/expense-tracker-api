using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;

using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public BudgetPlanService(DataContext dataContext, IMapper mapper)
        {
            _dbContext = dataContext;
            _mapper = mapper;
        }

        private IQueryable<BudgetPlan> GetBudgetPlansQueryable(){
            var query =  _dbContext.BudgetPlans.Where(sts => sts.isDeleted == false)
            .Include(ctx => ctx.Currency);
            return query;
        }

        private IQueryable<BudgetPlan> FilterBudgetPlans(IQueryable<BudgetPlan> query, BudgetPlanQueryDto budgetPlanQueryDto){
            var searchIsEmpty = budgetPlanQueryDto.SearchValue is null || string.IsNullOrEmpty(budgetPlanQueryDto.SearchValue);
            var userIdIsEmpty = budgetPlanQueryDto.UserId is null || string.IsNullOrEmpty(budgetPlanQueryDto.UserId);
            if(!searchIsEmpty){
                query = query.Where(exp => exp.Name.ToLower().Contains(budgetPlanQueryDto.SearchValue!.ToLower()));
            }
            if(!userIdIsEmpty){
                query = query.Where(exp => exp.User!.UserID.ToString() == budgetPlanQueryDto.UserId);
            }
            return query;
        }

        public async Task<ResponseBudgetPlanDto> AddBudgetPlan(AuthenticatedUser? authenticatedUser, AddBudgetPlanDto addBudgetPlanDto)
        {
            ResponseBudgetPlanDto response = new ResponseBudgetPlanDto();
            BudgetPlan? uniquePlan = await _dbContext.BudgetPlans.Where(
                budgetPlan => budgetPlan.UserId == authenticatedUser!.Id &&
                budgetPlan.Name == addBudgetPlanDto.Name && 
                budgetPlan.isDeleted == false
            ).FirstOrDefaultAsync();
            if(uniquePlan != null)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "There exists another budget plan with name "+uniquePlan.Name;
            }else{
                decimal accountAmount = addBudgetPlanDto.Amount - addBudgetPlanDto.BalanceAmount;
                Account? budgetPlanAccount = await new AccountHelper().getDefaultUserAccount(_dbContext,authenticatedUser!.Id);
                if(budgetPlanAccount == null){
                    budgetPlanAccount = await new AccountHelper().createUserDefaultAccount(_dbContext,authenticatedUser!);
                }
                if(budgetPlanAccount == null)
                {
                    response.Success = false;
                    response.Message = "An error occurred creating the budget plan. Try again later";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }else{
                    BudgetPlan budgetPlan = new BudgetPlan
                    {
                        Name = addBudgetPlanDto.Name,
                        UserId = authenticatedUser!.Id,
                        Description = addBudgetPlanDto.Description,
                        CurrencyID = addBudgetPlanDto.CurrencyID,
                        AccountID = budgetPlanAccount.Id,
                        StartDate = addBudgetPlanDto.StartDate,
                        EndDate = addBudgetPlanDto.EndDate,
                        Amount = addBudgetPlanDto.Amount,
                        BalanceAmount = addBudgetPlanDto.BalanceAmount,
                        isAchieved = addBudgetPlanDto.BalanceAmount == 0 ? true : false,
                        CreatedBy = authenticatedUser!.UserId,
                        Account = budgetPlanAccount
                    };
                    await _dbContext.BudgetPlans.AddAsync(budgetPlan);
                    await _dbContext.SaveChangesAsync();
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    response.Success = true;
                    response.Message = "Budget Plan Created Successfully";
                    response.Data = _mapper.Map<BudgetPlanDto>(budgetPlan);
                }
            }
            return response;
        }

        public async Task<ListResponseBudgetPlanDto> GetBudgetPlan(BudgetPlanQueryDto budgetPlanQueryDto)
        {
            ListResponseBudgetPlanDto response = new ListResponseBudgetPlanDto();
            var query = GetBudgetPlansQueryable();
            query = FilterBudgetPlans(query,budgetPlanQueryDto);
            var totalCount = await query.CountAsync();
            var pageSize = budgetPlanQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = budgetPlanQueryDto.PageNumber > 0 ? budgetPlanQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<BudgetPlan> budgetPlans = await query.ToListAsync();
            response.TotalPages = totalPages;
            response.TotalCount = totalCount;
            response.CurrentPage = currentPage;
            response.Success = true;
            response.Message = "Budget Plans fetched successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Results = _mapper.Map<List<BudgetPlanDto>>(budgetPlans);
            return response;
        }

        public async Task<ResponseBudgetPlanDto> UpdateBudgetPlan(Guid budgetPlanId, UpdateBudgetPlanDto updateBudgetPlanDto)
        {
            ResponseBudgetPlanDto response = new ResponseBudgetPlanDto();
            BudgetPlan? budgetPlanExists = await _dbContext.BudgetPlans.Where
            (
                budget => budget.isDeleted == false && budget.BudgetPlanID == budgetPlanId
            ).FirstOrDefaultAsync();
            if (budgetPlanExists == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "No such active Budget Plan exists";
            }else{
                budgetPlanExists.Name = updateBudgetPlanDto.Name;
                budgetPlanExists.Description = updateBudgetPlanDto.Description;;
                budgetPlanExists.CurrencyID = updateBudgetPlanDto.CurrencyID;
                budgetPlanExists.StartDate = updateBudgetPlanDto.StartDate;
                budgetPlanExists.EndDate =  updateBudgetPlanDto.EndDate;
                budgetPlanExists.Amount = updateBudgetPlanDto.Amount;
                budgetPlanExists.BalanceAmount = updateBudgetPlanDto.BalanceAmount;
                budgetPlanExists.isAchieved = updateBudgetPlanDto.BalanceAmount == 0 ? true : false;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Message = "Budget Plan updated successfully";
                response.Data = _mapper.Map<BudgetPlanDto>(budgetPlanExists);
            }
            return response;
        }

        public async Task<ResponseBudgetPlanDto> DeleteBudgetPlan(AuthenticatedUser? authenticatedUser,Guid budgetPlanId)
        {
            ResponseBudgetPlanDto response = new ResponseBudgetPlanDto();
            BudgetPlan? budgetPlanExists = await _dbContext.BudgetPlans.Where
            (
                budget => budget.isDeleted == false && budget.BudgetPlanID == budgetPlanId
            )
            .Include(budgt => budgt.Transactions!.Where(trans => trans.isDeleted == false).Take(1))
            .FirstOrDefaultAsync();
            if (budgetPlanExists == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "No such active Budget Plan exists";
            }else{
                if(budgetPlanExists.Transactions!.Count() > 0){
                    response.Success = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "There exists transactions on this budget plan";
                }else{
                    budgetPlanExists.isDeleted = true;
                    budgetPlanExists.DeletedAt = DateTime.UtcNow;
                    budgetPlanExists.DeletedBy = authenticatedUser!.UserId;
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = "Budget Plan deleted successfully";
                    response.Data = _mapper.Map<BudgetPlanDto>(budgetPlanExists);
                }
            }
            return response;
        }
    }
}