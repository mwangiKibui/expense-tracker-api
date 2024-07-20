namespace ExpenseTracker.DTO
{
    public class ResponseTransactionDto : ResponseDto
    {
        public TransactionDto? Data { get; set; }
    }
}