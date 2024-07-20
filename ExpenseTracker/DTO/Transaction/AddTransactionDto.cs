using ExpenseTracker.Data;

namespace ExpenseTracker.DTO
{
    
    public class AddTransactionDto
    {
        public int? ExpenseID { get; set; }
        public int? IncomeID { get; set; }
        public int? BudgetPlanID { get; set; }
        public required decimal Amount { get; set; }
        public required TransactionDefaults.AllowedTransactionTypes TransactionType {get;set;}
        public required TransactionDefaults.AllowedTransactionNatures TransactionNature {get;set;}
        public required DateOnly TransactionDate {get;set;}
        public string? Description { get; set; }
       /**
        in the next sprint, we will have the functionality to add a transaction to a bank account.
       **/
    }
}