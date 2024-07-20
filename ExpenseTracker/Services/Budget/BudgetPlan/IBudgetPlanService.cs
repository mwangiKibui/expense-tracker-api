using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface IBudgetPlanService
    {
        public Task<ResponseBudgetPlanDto> AddBudgetPlan(AuthenticatedUser? authenticatedUser, AddBudgetPlanDto addBudgetPlanDto);
        public Task<ListResponseBudgetPlanDto> GetBudgetPlan(BudgetPlanQueryDto budgetPlanQueryDto);
        public Task<ResponseBudgetPlanDto> UpdateBudgetPlan(Guid budgetPlanId, UpdateBudgetPlanDto updateBudgetPlanDto);
        public Task<ResponseBudgetPlanDto> DeleteBudgetPlan(AuthenticatedUser? authenticatedUser, Guid budgetPlanId);
    }
}