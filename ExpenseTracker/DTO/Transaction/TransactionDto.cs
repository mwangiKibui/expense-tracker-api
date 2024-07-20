using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.DTO
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public Guid TransactionID {get;set;}
        public TransactionDefaults.AllowedTransactionTypes TransactionType {get;set;}
        public TransactionDefaults.AllowedTransactionNatures TransactionNature {get;set;}
        public Expense? expense {get;set;}
        public Income? income {get;set;}
        public BudgetPlan? budgetPlan{get;set;}
        public User? user{get;set;}
        public required decimal Amount {get;set;}
        public required DateOnly TransactionDate {get;set;}
        public string? Description {get;set;}
        public Account? Account {get;set;}
    }
}