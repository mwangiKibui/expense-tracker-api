using ExpenseTracker.Models;

namespace ExpenseTracker.DTO
{
    public class BudgetPlanDto
    {
        public int Id { get; set;}
        public Guid BudgetPlanID {get;set;}
        public required string Name {get;set;}
        public User? User{get; set;}
        public required string Description {get;set;}
        public int? GoalType {get;set;}
        public BudgetGoalCategoryDto? BudgetGoalCategory {get; set;}
        public CurrencyDto? Currency {get; set;}
        public required DateOnly StartDate {get;set;}
        public required DateOnly EndDate {get;set;}
        public required decimal Amount {get;set;}
        public required decimal BalanceAmount {get;set;}
        public bool? isAchieved {get;set;}

    }
}