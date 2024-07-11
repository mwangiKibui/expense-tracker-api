using ExpenseTracker.EntityConfig;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){

        }

        // define the tables here.
        public DbSet<User> Users { set;get;}
        public DbSet<ForgotPasswordRequest> ForgotPasswordRequests{ set;get;}
        public DbSet<ExpenseType> ExpenseTypes { get;set;}
        public DbSet<Expense> Expenses { set;get;}
        public DbSet<ExpenseReminder> ExpenseReminders { set;get;}
        public DbSet<IncomeType> IncomeTypes { set;get;}
        public DbSet<Income> Incomes { set;get;}
        public DbSet<IncomeReminder> IncomeReminders {set;get;}
        public DbSet<Currency> Currencies { set;get;}
        public DbSet<BudgetGoalCategory> BudgetGoalCategories {set;get;}
        public DbSet<BudgetPlan> BudgetPlans { set;get;}
        public DbSet<Account> Accounts { set;get;}
        public DbSet<StagedTransaction> StagedTransactions { set;get;}
        public DbSet<Transaction> Transactions { set;get;}
        public DbSet<Notification> Notifications { set;get;}
        public DbSet<AuditLog> AuditLogs { set;get;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new ForgotPasswordRequestConfig());
            modelBuilder.ApplyConfiguration(new ExpenseTypeConfig());
            modelBuilder.ApplyConfiguration(new ExpenseConfig());
            modelBuilder.ApplyConfiguration(new IncomeTypeConfig());
            modelBuilder.ApplyConfiguration(new IncomeConfig());
            modelBuilder.ApplyConfiguration(new IncomeReminderConfig());
            modelBuilder.ApplyConfiguration(new CurrencyConfig());
            modelBuilder.ApplyConfiguration(new BudgetGoalCategoryConfig());
            modelBuilder.ApplyConfiguration(new BudgetPlanConfig());
            modelBuilder.ApplyConfiguration(new AccountConfig());
            modelBuilder.ApplyConfiguration(new StagedTransactionConfig());
            modelBuilder.ApplyConfiguration(new TransactionConfig());
            modelBuilder.ApplyConfiguration(new NotificationConfig());
            modelBuilder.ApplyConfiguration(new AuditLogConfig());
        }
    }
}