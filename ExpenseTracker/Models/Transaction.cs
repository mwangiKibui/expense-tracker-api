using ExpenseTracker.Data;

namespace ExpenseTracker.Models
{
    public class Transaction 
    {
        public int Id { get; set; }
        public Guid TransactionID { get; set; }
        public required TransactionDefaults.AllowedTransactionTypes TransactionType { get; set; }
        public required TransactionDefaults.AllowedTransactionNatures TransactionNature {get;set;}
        public int? ExpenseId { get; set; }
        public int? IncomeId { get; set; }
        public int? BudgetPlanId { get; set; }
        public required int UserId { get; set; }
        public required decimal Amount {get;set;}
        public required DateOnly TransactionDate {get; set; }
        public String? Description { get; set; }
        public String? Channel { get; set; }
        public required int AccountId { get; set; }
        public required int StagedTransactionId { get; set; }
        public bool? isDeleted {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public User? User { get; set; } = null;
        public Account? Account { get; set; } = null;
        public StagedTransaction? StagedTransaction{ get; set; } = null;
        public BudgetPlan? BudgetPlan {get;set;} = null;
        public Income? Income{ get; set; } = null;
        public Expense? Expense { get; set; } = null;
    }
}