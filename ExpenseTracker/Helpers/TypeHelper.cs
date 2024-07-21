using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Helpers
{
    public class TypeHelper
    {
        public async Task<bool> createUserDefaultExpenses(DataContext dbContext,int userId)
        {
            // fetch the system default expense types.
            List<ExpenseType> defaultExpenseTypes = await dbContext.ExpenseTypes.Where(
                expType => expType.SystemDefault == true
            ).ToListAsync();
            if(defaultExpenseTypes.Count == 0){ // no system default expenses.
                return true;
            }else{
                foreach(ExpenseType expenseType in defaultExpenseTypes)
                {
                    Expense newUserExpense = new Expense
                    {
                        ExpenseTypeId = expenseType.Id,
                        UserId = userId,
                        Name = expenseType.Name,
                        CreatedBy = expenseType.CreatedBy,
                    };
                    dbContext.Expenses.Add(newUserExpense);
                }
                await dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> createUserDefaultIncomes(DataContext dbContext,int userId)
        {
            // fetch the system default income types.
            List<IncomeType> defaultIncomesTypes = await dbContext.IncomeTypes.Where(
                expType => expType.SystemDefault == true
            ).ToListAsync();
            if(defaultIncomesTypes.Count == 0){ // no system default incomes.
                return true;
            }else{
                foreach(IncomeType incomeType in defaultIncomesTypes)
                {
                    Income newUserIncome = new Income
                    {
                        IncomeTypeId = incomeType.Id,
                        UserId = userId,
                        Name = incomeType.Name,
                        CreatedBy = incomeType.CreatedBy,
                    };
                    dbContext.Incomes.Add(newUserIncome);
                }
                await dbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}