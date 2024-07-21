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
        public ExpenseDto? expense {get;set;} = null;
        public IncomeDto? income {get;set;} = null;
        public BudgetPlanDto? budgetPlan{get;set;} = null;
        public SharedUserDto? user{get;set;} = null;
        public required decimal Amount {get;set;}
        public required DateOnly TransactionDate {get;set;}
        public string? Description {get;set;} = null;
        public AccountDto? Account {get;set;} = null;
    }
}