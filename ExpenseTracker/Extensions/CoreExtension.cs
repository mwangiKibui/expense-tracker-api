using ExpenseTracker.Mappings;
using ExpenseTracker.Services;
using Npgsql.Replication;

namespace ExpenseTracker.Extensions
{
    public static class CoreExtension
    {
        public static IServiceCollection AddCoreConfigs(this IServiceCollection services)
        {
            // Automapper staff.

            

            services.AddAutoMapper(typeof(IncomeTypeProfile));
            services.AddAutoMapper(typeof(ExpenseTypeProfile));
            services.AddAutoMapper(typeof(ExpenseProfile));
            services.AddAutoMapper(typeof(IncomeProfile));
            services.AddAutoMapper(typeof(CurrencyProfile));
            services.AddAutoMapper(typeof(BudgetGoalCategoryProfile));
            services.AddAutoMapper(typeof(AccountProfile));
            services.AddAutoMapper(typeof(BudgetPlanProfile));
            services.AddAutoMapper(typeof(TransactionProfile));
            services.AddAutoMapper(typeof(StagedTransactionProfile));

            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();

            services.AddControllers();

            return services;

        }
    }
}