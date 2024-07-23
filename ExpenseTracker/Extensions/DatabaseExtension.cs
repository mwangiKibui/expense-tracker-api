using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabaseExtension(this IServiceCollection services,IConfiguration configuration,IWebHostEnvironment environment)
        {
            string? dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            string? dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            string? dbName = Environment.GetEnvironmentVariable("DB_NAME");
            string? dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
            string? dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connectionString = environment.IsProduction() ? 
            configuration.GetConnectionString("ConnectionString")!
            .Replace("{DB_HOST}",dbHost)
            .Replace("{DB_PORT}",dbPort)
            .Replace("{DB_NAME}",dbName)
            .Replace("{DB_USERNAME}",dbUsername)
            .Replace("{DB_PASSWORD}",dbPassword)
            : configuration.GetConnectionString("ConnectionString");

            // add the db connection.
            services.AddDbContext<DataContext>(options => options.UseNpgsql(
                connectionString
                )
            );
            return services;
        } 
    }   
}