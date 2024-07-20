namespace ExpenseTracker.DTO
{
    public class ListResponseBudgetPlanDto : ListResponseDto
    {
        public List<BudgetPlanDto>? Results { get; set; }
    }
}