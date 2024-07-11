namespace ExpenseTracker.DTO
{
    public class ListResponseExpenseTypeDto  : ListResponseDto
    {   
        public List<ExpenseTypeDto>? Results { get; set; }
    }
}