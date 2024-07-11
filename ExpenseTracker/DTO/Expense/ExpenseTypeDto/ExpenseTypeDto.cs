namespace ExpenseTracker.DTO
{
    public class ExpenseTypeDto : CustomTypeDto
    {
        public int Id { get; set; }
        public Guid ExpenseTypeId { get; set; }

    }
}