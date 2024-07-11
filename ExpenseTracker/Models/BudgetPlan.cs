namespace ExpenseTracker.Models
{
    public class BudgetPlan 
    {
        public int Id { get; set; }
        public Guid BudgetPlanID { get; set; }
        public required String Name { get; set; }
        public required int UserId { get; set; }
        public required String Description { get; set; }
        public required int GoalType { get; set; }
        public int? BudgetCategoryID {get; set; }
        public required int CurrencyID { get; set; }
        public required DateTime StartDate {get; set; }
        public required DateTime EndDate {get; set; }
        public required decimal Amount {get;set;}
        public required decimal BalanceAmount {get;set;}
        public bool? isAchieved {get; set; } = false;
        public bool? isDeleted {get; set; } = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public User? User { get; set; } = null;
        public BudgetGoalCategory? BudgetGoalCategory { get; set; } = null;
        public Currency? Currency { get; set; } = null;
        public ICollection<Transaction>? Transactions { get; set; } = null;
    }
}