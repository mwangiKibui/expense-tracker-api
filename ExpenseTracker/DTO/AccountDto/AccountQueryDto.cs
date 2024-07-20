namespace ExpenseTracker.DTO
{
    public class AccountQueryDto : CustomQueryDto
    {
        public Guid? UserId { get; set; }
    }
}