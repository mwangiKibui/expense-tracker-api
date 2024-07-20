using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface IAccountService {
        Task<ResponseAccountDto> AddAccount(AuthenticatedUser? authenticatedUser,AddAccountDto addAccountDto);
        Task<ListResponseAccountDto> GetAccount(AuthenticatedUser? authenticatedUser,AccountQueryDto accountQueryDto);
        Task<ResponseAccountDto> UpdateAccount(AuthenticatedUser? authenticatedUser,Guid accountId,UpdateAccountDto updateAccountDto);
        Task<ResponseAccountDto> DeleteAccount(AuthenticatedUser? authenticatedUser,Guid accountId);
    }
}