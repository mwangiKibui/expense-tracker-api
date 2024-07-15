namespace ExpenseTracker.Models
{
    public class IncomeReminder
    {
        public int Id { get; set; }
        public Guid IncomeReminderID { get; set; }
        public required int IncomeId {get;set;}
        public required int UserId {get;set;}
        public required DateOnly DueDate {get;set;}
        public bool? isQueued {get;set;} = false;
        public bool? isDeleted {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public Income? Income {get;set;} = null;
        public User? User {get;set;} = null;
    }
}