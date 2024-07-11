namespace ExpenseTracker.DTO
{
    public class ListResponseIncomeTypeDto  : ListResponseDto
    {   
        public List<IncomeTypeDto>? Results { get; set; }
    }
}