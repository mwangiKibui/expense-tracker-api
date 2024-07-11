namespace ExpenseTracker.Models
{
    public class IncomeType 
    {
        public int Id { get; set; }
        public Guid IncomeTypeID { get; set; }
        public required String Name { get; set; }
        public String? Description { get; set; }
        public bool isDeleted {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public Guid CreatedBy {get;set;}
        public ICollection<Income>? Incomes { get; set; } = null;
    }
}