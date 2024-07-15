using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface IIncomeService{
        public Task<ResponseIncomeDto> AddIncome(AuthenticatedUser? authenticatedUser,AddIncomeDto addIncomeDto);
        public Task<ResponseIncomeDto> GetSingleIncome(Guid incomeId);
        public Task<ListResponseIncomeDto> GetIncomes(IncomeQueryDto incomeQueryDto);
        public Task<ResponseIncomeDto> UpdateIncome(Guid incomeId,UpdateIncomeDto updateIncomeDto);
        public Task<ResponseIncomeDto> DeleteIncome(AuthenticatedUser? authenticatedUser,Guid incomeId);
    }
}