using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency,CurrencyDto>();
        }
    }
}