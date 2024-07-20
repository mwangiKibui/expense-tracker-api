using ExpenseTracker.DTO;

namespace ExpenseTracker.Services
{
    public interface ITransactionService
    {
        public Task<ResponseTransactionDto> AddTransaction(AddTransactionDto addTransactionDto);
        public Task<ListResponseTransactionDto> GetTransaction(TransactionQueryDto transactionQueryDto);
        public Task<ResponseTransactionDto> UpdateTransaction(Guid transactionId,UpdateTransactionDto updateTransactionDto);
        public Task<ResponseTransactionDto> DeleteTransaction(Guid transactionId);
    }
}