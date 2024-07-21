namespace ExpenseTracker.DTO
{
    public class ReconcileStagedTransactionDto
    {
        public int StagedTransactionID {get;set;}
        public required List<AddTransactionDto> TransactionBreakdown {get;set;}
    }
}