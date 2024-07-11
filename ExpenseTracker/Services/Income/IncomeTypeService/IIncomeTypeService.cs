using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface IIncomeTypeService{
        public Task<ResponseIncomeTypeDto> AddIncomeType(AuthenticatedUser? authenticatedUser,AddIncomeTypeDto addIncomeTypeDto);
        public Task<ListResponseIncomeTypeDto> GetIncomeType(IncomeTypeQueryDto incomeTypeQueryDto);
        public Task<ResponseIncomeTypeDto> UpdateIncomeType(Guid incomeTypeId,UpdateIncomeTypeDto updateIncomeTypeDto);
        public Task<ResponseIncomeTypeDto> DeleteIncomeType(AuthenticatedUser? authenticatedUser,Guid incomeTypeId);
    }
}