namespace ExpenseTracker.DTO
{
    public class UpdateAccountDto
    {
        public int? AccountType { get; set; }
        public int? CurrencyId { get; set; }
        public String? Name { get; set; }
    }
}