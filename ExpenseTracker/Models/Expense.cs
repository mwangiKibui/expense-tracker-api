

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public Guid ExpenseID { get; set; }
        public required int ExpenseTypeId {get;set;}
        public int UserId {get;set;}
        public required String Name { get; set;}
        public bool isRecurring {get;set;} = false;
        public bool reminderEnabled {get;set;} = false;
        public int? NatureOfRecurrence {get;set;}
        public DateOnly? ReminderStartDate {get;set;}
        public bool IsDeleted {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public ExpenseType? ExpenseType{get;set;} = null;
        public User? User{get;set;} = null;
        public ICollection<ExpenseReminder>? ExpenseReminders {get; set;} = null;
        public ICollection<Transaction>? Transactions {get; set;} = null;
        
    }
}