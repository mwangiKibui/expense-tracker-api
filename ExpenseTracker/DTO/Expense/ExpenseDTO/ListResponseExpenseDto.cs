namespace ExpenseTracker.DTO
{
    public class ListResponseExpenseDto  : ListResponseDto
    {   
        public List<ExpenseDto>? Results { get; set; }
    }
}