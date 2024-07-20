

namespace ExpenseTracker.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public Guid CurrencyID {get;set;}
        public required String Name {get;set;}
        public required String Code {get;set;}
        public Boolean IsDefault {get;set;} = false;
        public Boolean IsDeleted { get; set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public ICollection<BudgetPlan>? BudgetPlans { get; set; } = null;
    }
}