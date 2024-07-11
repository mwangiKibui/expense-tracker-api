using ExpenseTracker.Models;

namespace ExpenseTracker.DTO {
    public class ExpenseDto {
        public int Id { get; set; }
        public Guid ExpenseID { get; set; }
        public required int ExpenseTypeId {get;set;}
        public int UserId {get;set;}
        public required String Name { get; set;}
        public bool isRecurring {get;set;} = false;
        public bool reminderEnabled {get;set;} = false;
        public int natureOfRecurrence {get;set;}
        public DateOnly? reminderStartDate {get;set;} = null;
        public bool isDeleted {get;set;} 
    }
}