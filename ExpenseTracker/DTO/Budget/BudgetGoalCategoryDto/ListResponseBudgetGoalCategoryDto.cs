namespace ExpenseTracker.DTO
{
    public class ListResponseBudgetGoalCategoryDto : ListResponseDto
    {
        public List<BudgetGoalCategory>? Results { get; set; }
    }
}