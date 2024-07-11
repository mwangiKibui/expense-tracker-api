using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class ForgotPasswordRequest
    {
        public int Id { get; set;}
        public Guid ForgotPasswordId { get; set; }
        public required int UserId {get;set;}

        public DateTime expiresOn {get;set;}

        public required string code {get;set;}

        public Boolean isActive {get;set;} = true;
        public User? User{get;set;} = null;
    }
}