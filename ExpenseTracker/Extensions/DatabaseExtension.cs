using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabaseExtension(this IServiceCollection services,IConfiguration configuration)
        {
            // add the db connection.
            services.AddDbContext<DataContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("ConnectionString"))
            );
            return services;
        } 
    }   
}