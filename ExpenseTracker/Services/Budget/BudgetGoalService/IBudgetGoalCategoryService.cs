using ExpenseTracker.DTO;

namespace ExpenseTracker.Services
{
    public interface IBudgetGoalCategoryService 
    {
        Task<ListResponseBudgetGoalCategoryDto> AddBudgetGoalCategory(List<AddBudgetGoalCategoryDto> addBudgetGoalCategoryDtos);
        Task<ListResponseBudgetGoalCategoryDto> GetBudgetGoalCategory();
        Task<ResponseBudgetGoalCategoryDto> UpdateBudgetGoalCategory(Guid budgetGoalCategoryId, UpdateBudgetGoalCategoryDto updateBudgetGoalCategoryDto);
        Task<ResponseBudgetGoalCategoryDto> DeleteBudgetGoalCategory(Guid budgetGoalCategoryId);
    }
}