namespace ExpenseTracker.DTO
{
    public class IncomeDto 
    {
        public int Id { get; set; }
        public Guid IncomeID { get; set; }
        public required int IncomeTypeId { get; set; }
        public required int UserId { get; set; }
        public required String Name { get; set; }
        public bool? IsRecurring { get; set; } = false;
        public bool? ReminderEnabled { get;set;} = false;
        public int? NatureOfRecurrence {get;set;}
        public DateOnly? ReminderStartDate {get;set;}
        public bool? isDeleted {get; set;}
    }
}