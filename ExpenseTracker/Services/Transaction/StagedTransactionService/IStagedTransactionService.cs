using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface IStagedTransactionService
    {
        public Task<ResponseStagedTransactionDto> AddStagedTransaction(AuthenticatedUser? authenticatedUser,AddStagedTransactionDto addStagedTransactionDto);
        public Task<ListResponseStagedTransactionDto> GetStagedTransaction(StagedTransactionQueryDto stagedTransactionQueryDto);
        public Task<ResponseStagedTransactionDto> DeleteStagedTransaction(AuthenticatedUser? authenticatedUser,Guid StagedTransactionId);
    }
}