using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class ExpenseType
    {
        public int Id { get; set; }
        public Guid ExpenseTypeId {get;set;}
        public required String Name {get;set;}
        public Boolean IsDeleted { get; set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}

    }
}