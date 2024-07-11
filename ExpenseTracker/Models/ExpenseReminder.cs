namespace ExpenseTracker.Models
{
    public class ExpenseReminder
    {
        public int Id { get; set; }
        public Guid ExpenseReminderID { get; set; }
        public required int ExpenseId {get;set;}
        public required int UserId {get;set;}
        public required int NatureOfRecurrence {get;set;}
        public required DateTime StartDate {get;set;}
        public required DateTime DueDate {get;set;}
        public bool? isQueued {get;set;} = false;
        public bool? isDeleted {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public Expense? Expense {get;set;} = null;
        public User? User {get;set;} = null;
    }
}