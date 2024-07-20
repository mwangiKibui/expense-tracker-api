
using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public class StagedTransactionProfile : Profile
    {
        public StagedTransactionProfile()
        {
            CreateMap<StagedTransaction,StagedTransactionDto>();
        }
    }
}