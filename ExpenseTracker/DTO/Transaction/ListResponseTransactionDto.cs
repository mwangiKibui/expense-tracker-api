namespace ExpenseTracker.DTO
{
    public class ListResponseTransactionDto : ListResponseDto
    {
        public List<TransactionDto>? Results { get; set; }
    }
}