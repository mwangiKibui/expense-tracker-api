using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class StagedTransactionService : IStagedTransactionService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public StagedTransactionService(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<StagedTransaction> GetStagedTransactionQueryable(){
            var query =  _dbContext.StagedTransactions.Where(sts => sts.isDeleted == false);
            return query;
        }

        private IQueryable<StagedTransaction> FilterStagedTransactions(IQueryable<StagedTransaction> query, StagedTransactionQueryDto stagedTransactionQueryDto){
            var channelIsEmpty = stagedTransactionQueryDto.Channel is null || string.IsNullOrEmpty(stagedTransactionQueryDto.Channel);
            var userIdIsEmpty = stagedTransactionQueryDto.UserId is null || string.IsNullOrEmpty(stagedTransactionQueryDto.UserId);
            var FromIsEmpty = stagedTransactionQueryDto.From is null;
            var ToIsEmpty = stagedTransactionQueryDto.To is null;
            var TransactionTypeIsEmpty = stagedTransactionQueryDto.TransactionType is null;
            var IsReconciledIsEmpty = stagedTransactionQueryDto.isReconciled is null;
            if(!channelIsEmpty){
                query = query.Where(stgTran => stgTran.Channel.ToLower().Contains(stagedTransactionQueryDto.Channel!.ToLower()));
            }
            if(!userIdIsEmpty){
                query = query.Where(stgTran => stgTran.User!.UserID.ToString() == stagedTransactionQueryDto.UserId);
            }
            if(!FromIsEmpty){
                query = query.Where(stgTran => stgTran.TransactionDate >= stagedTransactionQueryDto.From);
            }
            if(!ToIsEmpty){
                query = query.Where(stgTran => stgTran.TransactionDate <= stagedTransactionQueryDto.To);
            }
            if(!TransactionTypeIsEmpty){
                query = query.Where(stgTran => stgTran.TransactionType == stagedTransactionQueryDto.TransactionType);
            }
            if(!IsReconciledIsEmpty){
                query = query.Where(stgTran => stgTran.isReconciled == stagedTransactionQueryDto.isReconciled);
            }
            return query;
        }
        
        public async Task<ResponseStagedTransactionDto> AddStagedTransaction(AuthenticatedUser? authenticatedUser,AddStagedTransactionDto addStagedTransactionDto)
        {
            ResponseStagedTransactionDto response = new ResponseStagedTransactionDto();
            StagedTransaction? recordExists = await _dbContext.StagedTransactions.Where(
                stgTran => stgTran.TransactionCode == addStagedTransactionDto.TransactionCode &&
                stgTran.userId == addStagedTransactionDto.UserId && 
                stgTran.TransactionAmount == addStagedTransactionDto.TransactionAmount &&
                stgTran.TransactionDate == addStagedTransactionDto.TransactionDate
            ).FirstOrDefaultAsync();
            if(recordExists != null){
                response.Success = false;
                response.Message = "Duplicate record";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Data = _mapper.Map<StagedTransactionDto>(recordExists);
            }else{
                StagedTransaction stagedTransaction = new StagedTransaction{
                    userId = addStagedTransactionDto.UserId,
                    TransactionAmount = addStagedTransactionDto.TransactionAmount,
                    TransactionCode = addStagedTransactionDto.TransactionCode,
                    TransactionDate = addStagedTransactionDto.TransactionDate,
                    TransactionCurrency = addStagedTransactionDto.TransactionCurrency,
                    Channel = addStagedTransactionDto.channel,
                    TransactionType = addStagedTransactionDto.TransactionType,
                    CreatedBy = authenticatedUser!.UserId
                };
                await _dbContext.StagedTransactions.AddAsync(stagedTransaction);
                await _dbContext.SaveChangesAsync();
            }
            return response;
        }

        public async Task<ListResponseStagedTransactionDto> GetStagedTransaction(StagedTransactionQueryDto stagedTransactionQueryDto)
        {
            ListResponseStagedTransactionDto response = new ListResponseStagedTransactionDto();
            var query = GetStagedTransactionQueryable();
            query = FilterStagedTransactions(query,stagedTransactionQueryDto);
            if(stagedTransactionQueryDto.SortOrder == "DESC"){
                query = query.OrderByDescending(stgTran => stgTran.TransactionDate);
            }else{
                query = query.OrderBy(stgTran => stgTran.TransactionDate);
            }
            var totalCount = await query.CountAsync();
            var pageSize = stagedTransactionQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = stagedTransactionQueryDto.PageNumber > 0 ? stagedTransactionQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<StagedTransaction> results = await query.ToListAsync();
            response.Success = true;
            response.Message = "Staged Transactions Fetched Successfully";
            response.CurrentPage = currentPage;
            response.TotalCount = totalCount;
            response.TotalPages = totalPages;
            response.Results = _mapper.Map<List<StagedTransactionDto>>(results);
            return response;
        }

        public async Task<ResponseStagedTransactionDto> DeleteStagedTransaction(AuthenticatedUser? authenticatedUser,Guid stagedTransactionId){
            ResponseStagedTransactionDto response = new ResponseStagedTransactionDto();
            StagedTransaction? stagedTranExists = await _dbContext.StagedTransactions.Where(
                stgTran => stgTran.StagedTransactionID == stagedTransactionId &&
                stgTran.isDeleted == false
            ).Include(stgTran => stgTran.Transactions!.Where(tran => tran.isDeleted == false)!.Take(1))
            .FirstOrDefaultAsync();
            if(stagedTranExists == null){
                response.Success = false;
                response.Message = "No such active staged transaction exists";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }else{
                // check if there are any active transactions connected.
                if(stagedTranExists.Transactions!.Count() > 0){
                    response.Success = false;
                    response.Message = "There exists active transactions from this transaction";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }else{
                    stagedTranExists.isDeleted = false;
                    stagedTranExists.DeletedAt = DateTime.UtcNow;
                    stagedTranExists.DeletedBy = authenticatedUser!.UserId;
                    await _dbContext.SaveChangesAsync();
                } 
                response.Success = true;
                response.Message = "Staged Transaction Deleted Successfully";
                response.Data = _mapper.Map<StagedTransactionDto>(stagedTranExists);
            }
            return response;
        }
    }
}