namespace ExpenseTracker.DTO
{
    public class AddExpenseTypeDto : CustomTypeDto
    {   
        public bool? SystemDefault {get;set;} = false;
    }
}