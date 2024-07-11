

namespace ExpenseTracker.Models
{
    public class BudgetGoalCategory
    {
        public int Id { get; set; }
        public Guid BudgetGoalCategoryId {get;set;}
        public required String Name {get;set;}
        public String? Description {get;set;}
        public Boolean IsDeleted { get; set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public ICollection<BudgetPlan>? BudgetPlans { get; set; } = null;
    }
}