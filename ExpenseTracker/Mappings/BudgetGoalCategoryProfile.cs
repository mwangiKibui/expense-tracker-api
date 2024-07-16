using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class BudgetGoalCategoryProfile : Profile
    {
        public BudgetGoalCategoryProfile()
        {
            CreateMap<BudgetGoalCategory,BudgetGoalCategoryDto>();
        }
    }
}