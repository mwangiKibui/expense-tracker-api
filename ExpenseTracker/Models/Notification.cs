

namespace ExpenseTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public Guid NotificationID { get; set; }
        public required int UserId {get;set;}
        public required int NotificationType {get;set;}
        public String? DeviceId { get; set; }
        public required String Title { get; set;}
        public required String Message { get; set;}
        public required bool isSent {get;set;} = false;
        public required bool isRead {get;set;} = false;
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public User? User {get;set;} = null;
        
    }
}