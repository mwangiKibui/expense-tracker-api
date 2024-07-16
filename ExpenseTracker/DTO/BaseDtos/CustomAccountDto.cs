namespace ExpenseTracker.DTO
{
    public class CustomAccountDto
    {
        public required int AccountType { get; set; }
        public required int CurrencyId { get; set; }
        public required String Name { get; set; }
        public decimal? OpeningBalance { get; set; } = 0;
        public decimal? CurrentBalance  { get; set; } = 0;
    }
}