namespace ExpenseTracker.DTO
{
    public class ResponseStagedTransactionDto : ResponseDto
    {
        public StagedTransactionDto? Data { get; set; }
    }
}