namespace ExpenseTracker.DTO
{
    public class BudgetGoalCategory
    {
        public int Id { get; set; } 
        public Guid BudgetGoalCategoryId {get;set;}
        public required String Name {get;set;}
        public String? Description {get;set;}
    }
}