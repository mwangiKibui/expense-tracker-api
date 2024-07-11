using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services{
    public interface IExpenseService {
        public Task<ResponseExpenseDto> AddExpense (AuthenticatedUser? authenticatedUser,AddExpenseDto addExpenseDto);
        public Task<ResponseExpenseDto> GetSingleExpense (Guid expenseId);
        public Task<ListResponseExpenseDto> GetExpenses (ExpenseQueryDto expenseQueryDto);
        public Task<ResponseExpenseDto> UpdateExpense (Guid expenseId,UpdateExpenseDto updateExpenseDto);
        public Task<ResponseExpenseDto> DeleteExpense (AuthenticatedUser? authenticatedUser,Guid expenseTypeId);
    }
}