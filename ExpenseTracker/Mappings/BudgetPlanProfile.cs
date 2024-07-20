
using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;
using Newtonsoft.Json;

namespace ExpenseTracker.Mappings
{
    public class BudgetPlanProfile : Profile
    {
        public BudgetPlanProfile()
        {
            CreateMap<BudgetPlan,BudgetPlanDto>();
            // .ForMember(dest => dest.Currency,opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Currency)));
        }
    }
}