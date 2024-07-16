namespace ExpenseTracker.DTO
{
    public class ListResponseBudgetGoalCategoryDto : ListResponseDto
    {
        public List<BudgetGoalCategoryDto>? Results { get; set; }
    }
}