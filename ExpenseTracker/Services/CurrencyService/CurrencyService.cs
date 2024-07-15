using System.Runtime.CompilerServices;
using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class CurrencyService:ICurrencyService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public CurrencyService(DataContext dbContext,IMapper mapper){
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<Currency> GetCurrenciesQueryable(){
            var query =  _dbContext.Currencies.Where(sts => sts.IsDeleted == false);
            return query;
        }

        public async Task<ListResponseCurrencyDto> AddCurrency(AuthenticatedUser? authenticatedUser,List<AddCurrencyDto> addCurrencyDto){
            ListResponseCurrencyDto response = new ListResponseCurrencyDto();
            List<Currency> addedCurrencies = new List<Currency>();
            foreach(var currency in addCurrencyDto){
                // check if there exists currency with same code.
                Currency? existingCurrency = await _dbContext.Currencies.Where(curr => curr.Code.ToLower() == currency.Code.ToLower()).FirstOrDefaultAsync();
                if(existingCurrency != null){
                    addedCurrencies.Add(existingCurrency);
                    continue;
                }else{
                    Currency newCurrency = new Currency{
                        Name = currency.Name,
                        Code = currency.Code,
                        CreatedBy = authenticatedUser!.UserId,
                    };
                    await _dbContext.AddAsync(newCurrency);
                    addedCurrencies.Add(newCurrency);
                }
            }
            await _dbContext.SaveChangesAsync();
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Message = "Currencies created successfully";
            response.Results = _mapper.Map<List<CurrencyDto>>(addedCurrencies);
            return response;
        }

        public async Task<ListResponseCurrencyDto> GetCurrency(){
            ListResponseCurrencyDto response = new ListResponseCurrencyDto();
            var query = GetCurrenciesQueryable();
            List<Currency> currencies = await query.ToListAsync();
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = "Currencies fetched successfully";
            response.Results = _mapper.Map<List<CurrencyDto>>(currencies);
            return response;
        }

        public async Task<ResponseCurrencyDto> UpdateCurrency(Guid currencyId, UpdateCurrencyDto updateCurrencyDto){
            ResponseCurrencyDto response = new ResponseCurrencyDto();
            Currency? existingCurrency = await _dbContext.Currencies.Where(curr => curr.CurrencyID == currencyId).FirstOrDefaultAsync();
            if(existingCurrency == null){
                response.Success = false;
                response.Message = "No such currency exists";
            }else{
                existingCurrency.Name  = updateCurrencyDto.Name!;
                existingCurrency.Code = updateCurrencyDto.Code!;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Message = "Currency updated successfully";
                response.Data = _mapper.Map<CurrencyDto>(existingCurrency);
            }
            return response;
        }

        public async Task<ResponseCurrencyDto> DeleteCurrency(AuthenticatedUser? authenticatedUser,Guid currencyId){
            ResponseCurrencyDto response = new ResponseCurrencyDto();
            Currency? existingCurrency = await _dbContext.Currencies.Where(curr => curr.CurrencyID == currencyId).FirstOrDefaultAsync();
            if(existingCurrency == null){
                response.Success = false;
                response.Message = "No such active currency exists";
            }else{
                existingCurrency.IsDeleted = true;
                existingCurrency.DeletedBy = authenticatedUser!.UserId;
                existingCurrency.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Message = "Currency deleted successfully";
                response.Data = _mapper.Map<CurrencyDto>(existingCurrency);
            }
            return response;
        }

    }
}