namespace ExpenseTracker.DTO
{
    public class UpdateBudgetPlanDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int CurrencyID { get; set; }
        public required DateOnly StartDate {get;set;}
        public required DateOnly EndDate {get;set;}
        public required decimal Amount { get; set; }
        public required decimal BalanceAmount { get; set; }
         
    }
}