namespace ExpenseTracker.DTO
{
    public class ListResponseCurrencyDto  : ListResponseDto
    {   
        public List<CurrencyDto>? Results { get; set; }
    }
}