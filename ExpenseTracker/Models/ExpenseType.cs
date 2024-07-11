

namespace ExpenseTracker.Models
{
    public class ExpenseType
    {
        public int Id { get; set; }
        public Guid ExpenseTypeId {get;set;}
        public required String Name {get;set;}
        public String? Description {get;set;}
        public Boolean IsDeleted { get; set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public ICollection<Expense>? Expenses{get; set;} = null;
    }
}