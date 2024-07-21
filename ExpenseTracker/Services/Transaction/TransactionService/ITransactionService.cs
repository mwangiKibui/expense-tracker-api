using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface ITransactionService
    {
        public Task<ResponseTransactionDto> AddTransaction(AuthenticatedUser? authenticatedUser,AddTransactionDto addTransactionDto);
        public Task<ListResponseTransactionDto> ReconcileStagedTransaction(AuthenticatedUser? authenticatedUser,ReconcileStagedTransactionDto reconcileStagedTransactionDto);
        public Task<ListResponseTransactionDto> GetTransaction(TransactionQueryDto transactionQueryDto);
        public Task<ResponseTransactionDto> UpdateTransaction(Guid transactionId,AuthenticatedUser? authenticatedUser,UpdateTransactionDto updateTransactionDto);
        public Task<ResponseTransactionDto> DeleteTransaction(AuthenticatedUser? authenticatedUser,Guid transactionId);
    }
}