namespace ExpenseTracker.DTO
{
    public class AddIncomeDto 
    {
        public required Guid IncomeTypeId { get; set; }
        public required String Name { get; set; }
        public bool? IsRecurring { get; set; } = false;
        public bool? ReminderEnabled { get;set;} = false;
        public int? NatureOfRecurrence {get;set;}
        public DateOnly? ReminderStartDate {get;set;}
    }
}