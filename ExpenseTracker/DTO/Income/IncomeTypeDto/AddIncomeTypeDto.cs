namespace ExpenseTracker.DTO
{
    public class AddIncomeTypeDto : CustomTypeDto
    {   
        public bool? SystemDefault {get;set;} = false;
    }
}