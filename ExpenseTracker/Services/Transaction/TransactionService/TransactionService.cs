using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class TransactionValidity
    {
        public bool? validTransaction {get;set;} = true;
        public string? invalidTransactionMsg {get;set;}
        public BudgetPlan? budgetPlan {get;set;}
    }
    public class AddUpdateTransactionDto
    {
        public int? ExpenseID { get; set; }
        public int? IncomeID { get; set; }
        public int? BudgetPlanID { get; set; }
        public required TransactionDefaults.AllowedTransactionTypes TransactionType {get;set;}
        public required TransactionDefaults.AllowedTransactionNatures TransactionNature {get;set;}
    }
    public enum BudgetBalanceAction 
    {
        DECREASE = 1,
        INCREASE = 2
    }
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        

        public TransactionService(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private IQueryable<Transaction> GetTransactionsQueryable(){
            var query =  _dbContext.Transactions.Where(sts => sts.isDeleted == false);
            return query;
        }

        private IQueryable<Transaction> FilterTransactions(IQueryable<Transaction> query, TransactionQueryDto transactionQueryDto){
            var AccountIdIsEmpty = transactionQueryDto.AccountId is null;
            var UserIdIsEmpty = transactionQueryDto.UserId is null || string.IsNullOrEmpty(transactionQueryDto.UserId);
            var FromIsEmpty = transactionQueryDto.From is null;
            var ToIsEmpty = transactionQueryDto.To is null;
            var TransactionTypeIsEmpty = transactionQueryDto.TransactionType is null;
            var TransactionNatureIsEmpty = transactionQueryDto.TransactionNature is null;
            if(!AccountIdIsEmpty){
                query = query.Where(trn => trn.AccountId == transactionQueryDto.AccountId);
            }
            if(!UserIdIsEmpty){
                query = query.Where(trn => trn.User!.UserID.ToString() == transactionQueryDto.UserId);
            }
            if(!FromIsEmpty){
                query = query.Where(trn => trn.TransactionDate >= transactionQueryDto.From);
            }
            if(!ToIsEmpty){
                query = query.Where(trn => trn.TransactionDate <= transactionQueryDto.To);
            }
            if(!TransactionTypeIsEmpty){
                query = query.Where(trn => trn.TransactionType == transactionQueryDto.TransactionType);
            }
            if(!TransactionNatureIsEmpty){
                query = query.Where(trn => trn.TransactionNature == transactionQueryDto.TransactionNature);
            }
            return query;
        }

        private async Task<List<Transaction>> filterReconciledStagedTransactions(int stagedTransactionId)
        {
            List<Transaction>? transactions = await _dbContext.Transactions.Where(
                trn => trn.StagedTransactionId == stagedTransactionId &&
                trn.isDeleted == false
            ).ToListAsync();
            return transactions;
        }

        private async Task<Expense?> validateExpense(int expenseId,int userId)
        {
            Expense? expense = await _dbContext.Expenses.Where(
                exp => exp.Id == expenseId && 
                exp.UserId == userId
            ).FirstOrDefaultAsync();
            return expense;
        }

        private async Task<Income?> validateIncome(int incomeId,int userId)
        {
            Income? income = await _dbContext.Incomes.Where(
                inc => inc.Id == incomeId && 
                inc.UserId == userId
            ).FirstOrDefaultAsync();
            return income;
        }

        private async Task<BudgetPlan?> validateBudgetPlan(int budgetPlanId,int userId)
        {
            BudgetPlan? budgetPlan = await _dbContext.BudgetPlans.Where(
                budgPlan => budgPlan.Id == budgetPlanId && 
                budgPlan.UserId == userId
            ).FirstOrDefaultAsync();
            return budgetPlan;
        }

        private async Task<TransactionValidity> validateTransaction(AddUpdateTransactionDto addTransactionDto,int userId)
        {
            TransactionValidity result = new TransactionValidity();
            if(addTransactionDto.TransactionNature == TransactionDefaults.AllowedTransactionNatures.INCOME)
            {
                Income? incomeExists = await validateIncome((int) addTransactionDto.IncomeID!,userId);
                if(incomeExists == null)
                {
                    result.validTransaction = false;
                    result.invalidTransactionMsg = "No such active income exists";
                }
            }else if(addTransactionDto.TransactionNature == TransactionDefaults.AllowedTransactionNatures.EXPENSE)
            {
                Expense? expenseExists = await validateExpense((int) addTransactionDto.ExpenseID!,userId);
                if(expenseExists == null)
                {
                    result.validTransaction = false;
                    result.invalidTransactionMsg = "No such active expense exists";
                }
            }else{
                BudgetPlan? budgetPlanExists = await validateBudgetPlan((int) addTransactionDto.BudgetPlanID!,userId);
                if(budgetPlanExists == null)
                {
                    result.validTransaction = false;
                    result.invalidTransactionMsg = "No such active budget plan exists";
                }else{
                    result.budgetPlan = budgetPlanExists;
                }
            }
            return result;
        }

        private static BudgetPlan? balanceBudgetPlanBalance(BudgetPlan? budgetPlan,decimal transactionAmount,BudgetBalanceAction budgetBalanceAction = BudgetBalanceAction.DECREASE)
        {
            if(budgetPlan != null)
            {
                if(budgetPlan.BalanceAmount > 0)
                {
                    decimal amountVariable = 0;
                    if(budgetPlan.BalanceAmount >= transactionAmount)
                    {
                        amountVariable = transactionAmount;
                    }else{
                        amountVariable = budgetPlan.BalanceAmount;
                    }
                    if(budgetBalanceAction == BudgetBalanceAction.DECREASE)
                    {
                        budgetPlan.BalanceAmount = budgetPlan.Amount - amountVariable;
                    }else{
                        budgetPlan.BalanceAmount = budgetPlan.Amount + amountVariable;
                    }
                    if(budgetPlan.BalanceAmount == 0)
                    {
                        budgetPlan.isAchieved = true;
                    }
                }else{
                    budgetPlan.isAchieved = true;
                }
                return budgetPlan;
            }else{
                return null;
            }
        }

        public async Task<ResponseTransactionDto> AddTransaction(AuthenticatedUser? authenticatedUser,AddTransactionDto addTransactionDto)
        {
            ResponseTransactionDto response = new ResponseTransactionDto();
            Account? defaultUserAccount = await new AccountHelper().getDefaultUserAccount(_dbContext,authenticatedUser!.Id);
            if(defaultUserAccount == null)
            {
                defaultUserAccount = await new AccountHelper().createUserDefaultAccount(_dbContext,authenticatedUser);
            }
            if(defaultUserAccount == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "An error occurred recording transaction. Try again.";
            }else{
                // check if the details are valid based on the transaction nature.
                AddUpdateTransactionDto addUpdateTransactionDto = new AddUpdateTransactionDto {
                    ExpenseID = addTransactionDto.ExpenseID,
                    IncomeID = addTransactionDto.IncomeID,
                    BudgetPlanID = addTransactionDto.BudgetPlanID,
                    TransactionNature = addTransactionDto.TransactionNature,
                    TransactionType = addTransactionDto.TransactionType
                };
                TransactionValidity checker = await validateTransaction(addUpdateTransactionDto,authenticatedUser.Id);
                if(checker.validTransaction == false)
                {
                    response.Success = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = checker.invalidTransactionMsg;
                }else{
                    Transaction newTransaction = new Transaction // record the transaction
                    {
                        TransactionType = addTransactionDto.TransactionType,
                        TransactionNature = addTransactionDto.TransactionNature,
                        ExpenseId = addTransactionDto.ExpenseID,
                        IncomeId = addTransactionDto.IncomeID,
                        Amount = addTransactionDto.Amount,
                        TransactionDate = addTransactionDto.TransactionDate,
                        Description = addTransactionDto.Description,
                        UserId = authenticatedUser.Id,
                        AccountId = defaultUserAccount.Id
                    };
                    _dbContext.Transactions.Add(newTransaction);
                    // check if the transaction was for a budget plan.
                    if(addTransactionDto.TransactionNature == TransactionDefaults.AllowedTransactionNatures.BUDGETPLAN)
                    {
                        // update the balances
                        checker.budgetPlan = balanceBudgetPlanBalance(checker.budgetPlan,addTransactionDto.Amount);
                    }
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    response.Message = "Transaction recorded successfully";
                    response.Data = _mapper.Map<TransactionDto>(newTransaction);
                }
            }
            return response;
        }

        public async Task<ListResponseTransactionDto> ReconcileStagedTransaction(AuthenticatedUser? authenticatedUser,ReconcileStagedTransactionDto reconcileStagedTransactionDto)
        {
            ListResponseTransactionDto response = new ListResponseTransactionDto();
            Account? defaultUserAccount = await new AccountHelper().getDefaultUserAccount(_dbContext,authenticatedUser!.Id);
            if(defaultUserAccount == null)
            {
                defaultUserAccount = await new AccountHelper().createUserDefaultAccount(_dbContext, authenticatedUser);
            }
            if(defaultUserAccount == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "An error occurred reconciling transaction. Try again later";
            }else{
                List<Transaction> existingTransactions = await filterReconciledStagedTransactions(reconcileStagedTransactionDto.StagedTransactionID);
                if(existingTransactions.Count() > 0){
                    response.Success = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "There already exists active reconciled transactions.";
                }else{
                    List<Transaction> newTransactions = new List<Transaction>();
                    bool validTransactions = true;
                    foreach(AddTransactionDto addTransaction in reconcileStagedTransactionDto.TransactionBreakdown)
                    {
                        AddUpdateTransactionDto addUpdateTransactionDto = new AddUpdateTransactionDto {
                            ExpenseID = addTransaction.ExpenseID,
                            IncomeID = addTransaction.IncomeID,
                            BudgetPlanID = addTransaction.BudgetPlanID,
                            TransactionNature = addTransaction.TransactionNature,
                            TransactionType = addTransaction.TransactionType
                        };
                        TransactionValidity checker = await validateTransaction(addUpdateTransactionDto,authenticatedUser!.Id);
                        if(checker.validTransaction == false)
                        {
                            response.Success = false;
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            response.Message = checker.invalidTransactionMsg;
                            validTransactions = false;
                            break;
                        }else{
                            newTransactions.Add(new Transaction // record the transaction
                            {
                                TransactionType = addTransaction.TransactionType,
                                TransactionNature = addTransaction.TransactionNature,
                                ExpenseId = addTransaction.ExpenseID,
                                IncomeId = addTransaction.IncomeID,
                                Amount = addTransaction.Amount,
                                TransactionDate = addTransaction.TransactionDate,
                                Description = addTransaction.Description,
                                UserId = authenticatedUser.Id,
                                AccountId = defaultUserAccount.Id,
                                StagedTransactionId = reconcileStagedTransactionDto.StagedTransactionID,
                                BudgetPlan = checker.budgetPlan
                            });
                        }
                    }
                    if(validTransactions)
                    {
                        foreach(Transaction transaction in newTransactions)
                        {
                            _dbContext.Transactions.Add(transaction);
                            if(transaction.BudgetPlan != null)
                            {
                                transaction.BudgetPlan = balanceBudgetPlanBalance(transaction.BudgetPlan,transaction.Amount);
                            }
                            await _dbContext.SaveChangesAsync();
                        }
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = "Transaction reconciled successfully";
                        response.Success = true;
                        response.Results = _mapper.Map<List<TransactionDto>>(newTransactions);
                    }else{
                        // response already set.
                    }
                }
            }
            return response;
        }

        public async Task<ListResponseTransactionDto> GetTransaction(TransactionQueryDto transactionQueryDto)
        {
            ListResponseTransactionDto response = new ListResponseTransactionDto();
            var query = GetTransactionsQueryable();
            query = FilterTransactions(query,transactionQueryDto);
            if(transactionQueryDto.SortOrder == "DESC")
            {
                query = query.OrderByDescending(transaction => transaction.TransactionDate);
            }else{
                query = query.OrderBy(transaction => transaction.TransactionDate);
            }
            var totalCount = await query.CountAsync();
            var pageSize = transactionQueryDto.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var currentPage = transactionQueryDto.PageNumber > 0 ? transactionQueryDto.PageNumber : 1;
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            List<Transaction> results = await query.ToListAsync();
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = "Transactions fetched successfully";
            response.CurrentPage = currentPage;
            response.TotalCount = totalCount;
            response.TotalPages = totalPages;
            response.Results = _mapper.Map<List<TransactionDto>>(results);
            return response;
        }

        public async Task<ResponseTransactionDto> UpdateTransaction(Guid transactionId,AuthenticatedUser? authenticatedUser,UpdateTransactionDto updateTransactionDto)
        {
            ResponseTransactionDto response = new ResponseTransactionDto();
            // check if the transaction exists.
            Transaction? transactionExists = await _dbContext.Transactions.Where(
                trn => trn.TransactionID == transactionId &&
                trn.isDeleted == false
            ).FirstOrDefaultAsync();
            if(transactionExists is null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "No such active transaction exists";
            }else{
                AddUpdateTransactionDto addUpdateTransactionDto = new AddUpdateTransactionDto {
                    ExpenseID = updateTransactionDto.ExpenseID,
                    IncomeID = updateTransactionDto.IncomeID,
                    BudgetPlanID = updateTransactionDto.BudgetPlanID,
                    TransactionNature = updateTransactionDto.TransactionNature,
                    TransactionType = updateTransactionDto.TransactionType
                };
                TransactionValidity checker = await validateTransaction(addUpdateTransactionDto,authenticatedUser!.Id);
                if(checker.validTransaction == false){
                    response.Success = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = checker.invalidTransactionMsg;
                }else{
                    if(transactionExists.Amount != updateTransactionDto.Amount && checker.budgetPlan != null)
                    {
                        // update the budget plan too.
                        checker.budgetPlan = balanceBudgetPlanBalance(checker.budgetPlan,updateTransactionDto.Amount);
                    }
                    transactionExists.ExpenseId = updateTransactionDto.ExpenseID;
                    transactionExists.IncomeId = updateTransactionDto.IncomeID;
                    transactionExists.BudgetPlanId = updateTransactionDto.BudgetPlanID;
                    transactionExists.Amount = updateTransactionDto.Amount;
                    transactionExists.TransactionType = updateTransactionDto.TransactionType;
                    transactionExists.TransactionNature = updateTransactionDto.TransactionNature;
                    transactionExists.TransactionDate = updateTransactionDto.TransactionDate;
                    transactionExists.Description = updateTransactionDto.Description;
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Transaction updated successfully";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Data = _mapper.Map<TransactionDto>(transactionExists);
                }
            }
            return response;
        }

        public async Task<ResponseTransactionDto> DeleteTransaction(AuthenticatedUser? authenticatedUser,Guid transactionId)
        {
            ResponseTransactionDto response = new ResponseTransactionDto();
            // check if the transaction is active.
            Transaction? transactionExists = await _dbContext.Transactions.Where(
                trn => trn.TransactionID == transactionId &&
                trn.isDeleted == false
            ).Include(trn => trn.BudgetPlan)
            .FirstOrDefaultAsync();
            if(transactionExists == null)
            {
                response.Success = false;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "No such active transaction exists";
            }else{
                if(transactionExists.BudgetPlan is not null)
                { // update the budget plan budgets.
                    transactionExists.BudgetPlan = balanceBudgetPlanBalance(transactionExists.BudgetPlan,transactionExists.Amount);
                }
                transactionExists.isDeleted = true;
                transactionExists.DeletedAt = DateTime.UtcNow;
                transactionExists.DeletedBy = authenticatedUser!.UserId;
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Message = "Transaction deleted successfully";
            }
            return response;
        }

    }
}