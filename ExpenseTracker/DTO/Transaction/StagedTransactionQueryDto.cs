using ExpenseTracker.Data;

namespace ExpenseTracker.DTO
{
    public class StagedTransactionQueryDto : CustomQueryDto
    {  
        public string? UserId {get;set;}
        public string? Channel {get;set;}
        public DateOnly? From {get;set;}
        public DateOnly? To {get;set;}
        public TransactionDefaults.AllowedTransactionTypes? TransactionType {get;set;}
        public bool? isReconciled {get;set;}
    }
}