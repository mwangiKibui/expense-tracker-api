

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public Guid ExpenseID { get; set; }
        public required int ExpenseTypeId {get;set;}
        public int UserId {get;set;}
        public required String Name { get; set;}
        public required bool isRecurring {get;set;} = false;
        public required bool reminderEnabled {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public ExpenseType? ExpenseType{get;set;} = null;
        public User? User{get;set;} = null;
        public ICollection<ExpenseReminder>? ExpenseReminders {get; set;} = null;
        public ICollection<Transaction>? Transactions {get; set;} = null;
        
    }
}