namespace ExpenseTracker.DTO
{
    public class SharedUserDto
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int Id { get; set; }
        public required int UserId { get; set; }
    }
}