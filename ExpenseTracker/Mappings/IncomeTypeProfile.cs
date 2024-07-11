using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class IncomeTypeProfile : Profile
    {
        public IncomeTypeProfile()
        {
            CreateMap<IncomeType,IncomeTypeDto>();
        }
    }
}