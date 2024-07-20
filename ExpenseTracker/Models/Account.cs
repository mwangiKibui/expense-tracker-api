namespace ExpenseTracker.Models
{
    public class Account 
    {
        public int Id { get; set;}
        public Guid AccountID {get;set;}
        public required int AccountType {get;set;}
        public required int CurrencyId {get;set;}
        public required int UserId {get;set;}
        public required String Name {get;set;}
        public required decimal OpeningBalance {get;set;}
        public required decimal CurrentBalance {get;set;}
        public bool? isDeleted {get; set; } = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public Currency? Currency {get;set;} = null;
        public ICollection<Transaction>? Transactions {get; set;} = null;
        public User? User {get;set;} = null;
        public ICollection<BudgetPlan>? BudgetPlans {get;set;} = null;
    }
}