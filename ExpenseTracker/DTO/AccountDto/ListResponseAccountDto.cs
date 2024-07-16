namespace ExpenseTracker.DTO
{
    public class ListResponseAccountDto : ListResponseDto
    {
        public List<AccountDto>? Results { get; set; }
    }
}