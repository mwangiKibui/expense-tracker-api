namespace ExpenseTracker.Models
{
    public class User
    {
        public int Id { get; set;}
        public Guid UserID { get; set; }
        public required string FirstName { get; set;}
        public required string MiddleName { get; set;}
        public required string LastName { get; set;}
        public required string Email {get;set;}
        public required string Password {get;set;}
        public ICollection<ForgotPasswordRequest>? ForgotPasswordRequests { get; set;} = null;
        public ICollection<ExpenseReminder>? ExpenseReminders { get; set;} = null;
        public ICollection<IncomeReminder>? IncomeReminders { get; set;} = null;
        public ICollection<Income>? Incomes { get; set;} = null;
        public ICollection<Expense>? Expenses { get; set;} = null;
        public ICollection<BudgetPlan>? BudgetPlans { get; set;} = null;
        public ICollection<Transaction>? Transactions { get; set;} = null;
        public ICollection<Notification>? Notifications { get; set;} = null;
        public ICollection<AuditLog>? AuditLogs{ get; set;} = null;
        
    }
}