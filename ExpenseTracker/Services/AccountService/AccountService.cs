using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public AccountService(DataContext dbContext,IMapper mapper){
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<Account> GetAccountsQueryable(){
            var query =  _dbContext.Accounts.Where(sts => sts.isDeleted == false);
            return query;
        }

        public async Task<ResponseAccountDto> AddAccount(AuthenticatedUser? authenticatedUser,AddAccountDto addAccountDto){
            ResponseAccountDto response = new ResponseAccountDto();
            Account? accountExists = await _dbContext.Accounts.Where(acc => acc.UserId == authenticatedUser!.Id && acc.Name  == addAccountDto.Name).FirstOrDefaultAsync();
            if(accountExists != null){
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Account with name "+accountExists.Name+" already exists";
                response.Data = _mapper.Map<AccountDto>(accountExists);
            }else{
                Account newAccount = new Account {
                    AccountType = addAccountDto.AccountType,
                    CurrencyId = addAccountDto.CurrencyId,
                    UserId = authenticatedUser!.Id,
                    Name = addAccountDto.Name,
                    OpeningBalance = addAccountDto.OpeningBalance == null ? 0 : (decimal)addAccountDto.OpeningBalance,
                    CurrentBalance = addAccountDto.CurrentBalance == null ? 0 : (decimal)addAccountDto.CurrentBalance,
                    CreatedBy = authenticatedUser.UserId
                };
                await _dbContext.AddAsync(newAccount);
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.Created;
                response.Message = "Account created successfully";
                response.Data = _mapper.Map<AccountDto>(newAccount);
            }
            return response;
        }
    

    public async Task<ListResponseAccountDto> GetAccount(AuthenticatedUser? authenticatedUser,AccountQueryDto accountQueryDto)
    {
        var response = new ListResponseAccountDto();
        var query = GetAccountsQueryable();
        if(!String.IsNullOrEmpty(accountQueryDto.UserId.ToString())){
            User? user = await _dbContext.Users.Where(usr => usr.UserID == accountQueryDto.UserId).FirstOrDefaultAsync();
            query = query.Where(acc => acc.UserId == user!.Id);
        }else{ // filter for the current logged in user.
            query = query.Where(acc => acc.UserId == authenticatedUser!.Id);
        }
        var totalCount = await query.CountAsync();
        var pageSize = accountQueryDto.PageSize;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var currentPage = accountQueryDto.PageNumber > 0 ? accountQueryDto.PageNumber : 1;
        var skip = (currentPage - 1) * pageSize;
        query = query.Skip(skip).Take(pageSize);
        List<Account> accounts = await query.ToListAsync();
        response.Success = true;
        response.Message = "Accounts fetched successfully";
        response.StatusCode = System.Net.HttpStatusCode.OK;
        response.Results = _mapper.Map<List<AccountDto>>(accounts);
        response.CurrentPage = currentPage;
        response.TotalCount = totalCount;
        response.TotalPages = totalPages;
        return response;
    }

    public async Task<ResponseAccountDto> UpdateAccount(AuthenticatedUser? authenticatedUser, Guid accountId, UpdateAccountDto updateAccountDto)
    {
        var response = new ResponseAccountDto();
        Account? accountExists = await _dbContext.Accounts.Where(acc => acc.AccountID == accountId && acc.isDeleted == false).FirstOrDefaultAsync();
        if(accountExists == null){
            response.Success = false;
            response.Message = "No such account exists";
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }else{
            if(accountExists.CreatedBy != authenticatedUser!.UserId){
                response.Success = false;
                response.Message = "Account can only be updated by the owner";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }else{
                accountExists.Name = updateAccountDto.Name!;
                accountExists.AccountType = (int)updateAccountDto.AccountType!;
                accountExists.CurrencyId = (int)updateAccountDto.CurrencyId!;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Account updated successfully";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<AccountDto>(accountExists);
            }
        }
        return response;
    }  

    public async Task<ResponseAccountDto> DeleteAccount(AuthenticatedUser? authenticatedUser,Guid accountId)
    {
        var response = new ResponseAccountDto();
        Account? accountExists = await _dbContext.Accounts.
        Where(acc => acc.AccountID == accountId && acc.isDeleted == false)
        .Include(
            acc => acc.Transactions!.Where(
                transaction => transaction.isDeleted == false
            )!.Take(1))
        .Include(
            acc => acc.BudgetPlans!.Where(
                budgetPlan => budgetPlan.isDeleted == false
            )!.Take(1)
        )
        .FirstOrDefaultAsync();
        if(accountExists == null){
            response.Success = false;
            response.Message = "No such account exists";
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }else{
            if(accountExists.Transactions!.Count() > 0){
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "There already exists active transactions in this account";
            }else if(accountExists.BudgetPlans!.Count() > 0){
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "There already an active budget plan in this account";
            }else{
                // delete the account.
                accountExists.DeletedAt = DateTime.UtcNow;
                accountExists.DeletedBy = authenticatedUser!.UserId;
                accountExists.isDeleted = true;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Account deleted successfully";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = _mapper.Map<AccountDto>(accountExists);
            }
        }
        return response;
    } 
}

}