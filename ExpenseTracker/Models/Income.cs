namespace ExpenseTracker.Models
{
    public class Income 
    {
        public int Id { get; set; }
        public Guid IncomeID { get; set; }
        public required int IncomeTypeId { get; set; }
        public required int UserId { get; set; }
        public required String Name { get; set; }
        public bool? isRecurring { get; set; } = false;
        public bool? reminderEnabled { get;set;} = false;
        public int? NatureOfRecurrence {get;set;}
        public DateOnly? ReminderStartDate {get;set;}
        public bool? isDeleted {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public IncomeType? IncomeType { get; set; } = null;
        public User? User { get; set; } = null;
        public ICollection<IncomeReminder>? IncomeReminders { get; set; } = null;
        public ICollection<Transaction>? Transactions { get; set; } = null;
    }
}