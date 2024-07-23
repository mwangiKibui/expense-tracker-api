using ExpenseTracker.Services;

namespace ExpenseTracker.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IIncomeTypeService, IncomeTypeService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IBudgetGoalCategoryService, BudgetGoalCategoryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBudgetPlanService, BudgetPlanService>();
            services.AddScoped<IStagedTransactionService, StagedTransactionService>();
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }
    }
}