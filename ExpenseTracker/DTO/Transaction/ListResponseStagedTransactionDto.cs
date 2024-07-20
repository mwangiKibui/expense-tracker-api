namespace ExpenseTracker.DTO
{
    public class ListResponseStagedTransactionDto : ListResponseDto
    {
        public List<StagedTransactionDto>? Results { get; set; }
    }
}