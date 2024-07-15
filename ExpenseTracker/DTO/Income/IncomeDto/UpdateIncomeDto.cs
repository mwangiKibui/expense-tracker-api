namespace ExpenseTracker.DTO
{
    public class UpdateIncomeDto 
    {   
        public int? IncomeTypeId { get; set; }  
        public bool? IsRecurring { get; set; }
        public bool? ReminderEnabled {get;set;}
        public string? Name {get;set;}
        public int? NatureOfRecurrence {get;set;}
        public DateOnly? ReminderStartDate {get;set;}

    }
}