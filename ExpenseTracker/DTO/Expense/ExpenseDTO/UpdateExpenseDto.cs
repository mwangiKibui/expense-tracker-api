namespace ExpenseTracker.DTO
{
    public class UpdateExpenseDto 
    {   
        public int? ExpenseTypeId { get; set; }  
        public bool? isRecurring { get; set; }
        public bool? reminderEnabled {get;set;}
        public string? Name {get;set;}
        public int? natureOfRecurrence {get;set;}
        public DateOnly? reminderStartDate {get;set;}

    }
}