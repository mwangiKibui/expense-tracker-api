namespace ExpenseTracker.Models
{
    public class User
    {
        public int Id { get; set;}
        public Guid UserID { get; set; }
        public required string FirstName { get; set;}
        public required string MiddleName { get; set;}
        public required string LastName { get; set;}
        public required string Email {get;set;}
        public required string Password {get;set;}
    }
}