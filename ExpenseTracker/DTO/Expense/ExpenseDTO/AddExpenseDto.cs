namespace ExpenseTracker.DTO {
    public class AddExpenseDto {
        public required Guid ExpenseTypeId { get; set; }
        public required string Name { get; set; }
        public bool isRecurring { get; set; } = false;
        public bool reminderEnabled { get; set; } = false;
        public int? natureOfRecurrence {get;set;}
        public DateOnly? reminderStartDate {get;set;}
    }
}