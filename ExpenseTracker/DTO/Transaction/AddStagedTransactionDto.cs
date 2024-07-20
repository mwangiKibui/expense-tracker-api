using ExpenseTracker.Data;

namespace ExpenseTracker.DTO
{
    public class AddStagedTransactionDto
    {
        public required string TransactionCode { get; set; }
        public required DateOnly TransactionDate {get;set;}
        public required int UserId { get; set;}
        public required TransactionDefaults.AllowedTransactionTypes TransactionType {get;set;}
        public required decimal TransactionAmount {get;set;}
        public required string TransactionCurrency {get;set;}
        public required string channel {get;set;}
    }
}