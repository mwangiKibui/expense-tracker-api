

namespace ExpenseTracker.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public Guid AuditLogID { get; set; }
        public required int UserId {get;set;}
        public required String ActionType {get;set;}
        public required bool Success {get;set;} = true;
        public String? Payload { get; set; }
        public String? Response { get; set;}
        public String? Error { get; set;}
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public User? User {get;set;} = null;
        
    }
}