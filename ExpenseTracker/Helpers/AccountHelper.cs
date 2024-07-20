using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Helpers
{
    public class AccountHelper 
    {
        public async Task<Account?> createUserDefaultAccount(Data.DataContext dbContext, User user)
        {
            Currency? defaultCurrency = await dbContext.Currencies.Where(
                c => c.IsDefault == true
            ).FirstOrDefaultAsync();
            if(defaultCurrency == null){
                defaultCurrency = await dbContext.Currencies.Where(c => c.Code == "KES").FirstOrDefaultAsync();
            }
            if(defaultCurrency == null){
                return null;
            }else{
                Account newUserAccount = new Account{
                    CurrencyId = defaultCurrency.Id,
                    Name = user!.FirstName[0] + ' ' + user!.LastName[0] + " E.W",
                    OpeningBalance = 0,
                    CurrentBalance = 0,
                    AccountType = new AccountTypes().DEFAULT_ACCOUNT_TYPE,
                    UserId = user.Id,
                    CreatedBy = user.UserID
                };
                await dbContext.Accounts.AddAsync(newUserAccount);
                await dbContext.SaveChangesAsync();
                return newUserAccount;
            }
        }

        public async Task<Account?> createUserDefaultAccount(Data.DataContext dbContext, AuthenticatedUser authenticatedUser)
        {
            Currency? defaultCurrency = await dbContext.Currencies.Where(
                c => c.IsDefault == true
            ).FirstOrDefaultAsync();
            if(defaultCurrency == null){
                defaultCurrency = await dbContext.Currencies.Where(c => c.Code == "KES").FirstOrDefaultAsync();
            }
            if(defaultCurrency == null){
                return null;
            }else{
                Account newUserAccount = new Account{
                    CurrencyId = defaultCurrency.Id,
                    Name = authenticatedUser!.FirstName.First().ToString().ToUpper() + "" + authenticatedUser!.LastName.First().ToString().ToUpper() + " E.W",
                    OpeningBalance = 0,
                    CurrentBalance = 0,
                    AccountType = new AccountTypes().DEFAULT_ACCOUNT_TYPE,
                    UserId = authenticatedUser.Id,
                    CreatedBy = authenticatedUser.UserId
                };
                await dbContext.Accounts.AddAsync(newUserAccount);
                await dbContext.SaveChangesAsync();
                return newUserAccount;
            }
        }

        public async Task<Account?> getDefaultUserAccount(Data.DataContext dbContext, int userId)
        {
            Account? userDefaultAccount = await dbContext.Accounts.Where(
                acc => acc.UserId == userId && 
                acc.AccountType == new AccountTypes().DEFAULT_ACCOUNT_TYPE &&
                acc.isDeleted == false
            ).FirstOrDefaultAsync();
            return userDefaultAccount;
        }
    }
}