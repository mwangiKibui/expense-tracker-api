namespace ExpenseTracker.DTO
{
    public class AccountDto : CustomAccountDto
    {
        public int Id { get; set; }
        public Guid AccountID { get; set; }
    }
}