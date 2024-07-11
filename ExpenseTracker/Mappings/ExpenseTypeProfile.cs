using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class ExpenseTypeProfile : Profile
    {
        public ExpenseTypeProfile()
        {
            CreateMap<ExpenseType,ExpenseTypeDto>();
        }
    }
}