using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{

    public interface IExpenseTypeService
    {
        public Task<ResponseExpenseTypeDto> AddExpenseType(AuthenticatedUser? authenticatedUser,AddExpenseTypeDto addExpenseTypeDto);
        public Task<ListResponseExpenseTypeDto> GetExpenseType(ExpenseTypeQueryDto expenseTypeQueryDto);
        public Task<ResponseExpenseTypeDto> UpdateExpenseType(Guid expenseTypeId,UpdateExpenseTypeDto updateExpenseTypeDto);
        public Task<ResponseExpenseTypeDto> DeleteExpenseType(AuthenticatedUser? authenticatedUser,Guid expenseTypeId);
    }
}