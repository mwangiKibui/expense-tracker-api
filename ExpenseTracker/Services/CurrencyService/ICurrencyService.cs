using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;

namespace ExpenseTracker.Services
{
    public interface ICurrencyService {
        public Task<ListResponseCurrencyDto> AddCurrency(AuthenticatedUser? authenticatedUser,List<AddCurrencyDto> addCurrencyDto);
        public Task<ListResponseCurrencyDto> GetCurrency();
        public Task<ResponseCurrencyDto> UpdateCurrency(Guid currencyId, UpdateCurrencyDto updateCurrencyDto);
        public Task<ResponseCurrencyDto> DeleteCurrency(AuthenticatedUser? authenticatedUser,Guid currencyId);
    }
}