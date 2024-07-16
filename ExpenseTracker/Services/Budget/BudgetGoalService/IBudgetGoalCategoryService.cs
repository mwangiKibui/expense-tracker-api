using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface IBudgetGoalCategoryService 
    {
        Task<ListResponseBudgetGoalCategoryDto> AddBudgetGoalCategory(AuthenticatedUser? authenticatedUser,List<AddBudgetGoalCategoryDto> addBudgetGoalCategoryDtos);
        Task<ListResponseBudgetGoalCategoryDto> GetBudgetGoalCategory(CustomQueryDto customQueryDto);
        Task<ResponseBudgetGoalCategoryDto> UpdateBudgetGoalCategory(Guid budgetGoalCategoryId, UpdateBudgetGoalCategoryDto updateBudgetGoalCategoryDto);
        Task<ResponseBudgetGoalCategoryDto> DeleteBudgetGoalCategory(AuthenticatedUser? authenticatedUser,Guid budgetGoalCategoryId);
    }
}