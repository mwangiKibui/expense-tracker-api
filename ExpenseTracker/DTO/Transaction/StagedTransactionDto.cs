
using ExpenseTracker.Models;

namespace ExpenseTracker.DTO
{
    public class StagedTransactionDto
    {
        public int Id { get; set; }
        public Guid StagedTransactionID {get;set;}
        public string? TransactionCode {get;set;}
        public bool? isReconciled {get;set;}
        public required string Channel{get;set;}
        public User? User{get;set;}
        public required decimal TransactionAmount {get;set;}
        public required string TransactionCurrency {get;set;}
        public required DateOnly TransactionDate {get;set;}
    }
}