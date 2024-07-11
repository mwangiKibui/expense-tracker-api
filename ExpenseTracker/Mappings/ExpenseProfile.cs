using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense,ExpenseDto>();
        }
    }
}