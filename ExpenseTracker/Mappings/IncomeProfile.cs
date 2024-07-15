using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class IncomeProfile : Profile
    {
        public IncomeProfile()
        {
            CreateMap<Income,IncomeDto>();
        }
    }
}