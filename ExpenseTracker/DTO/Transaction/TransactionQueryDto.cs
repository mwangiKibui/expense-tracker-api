using ExpenseTracker.Data;

namespace ExpenseTracker.DTO
{
    public class TransactionQueryDto : CustomQueryDto
    {  
        public string? UserId {get;set;}
        public TransactionDefaults.AllowedTransactionTypes? TransactionType {get;set;}
        public TransactionDefaults.AllowedTransactionNatures? TransactionNature {get;set;}
        public DateOnly? From {get;set;}
        public DateOnly? To {get;set;}
        public int? AccountId {get;set;}
    }
}