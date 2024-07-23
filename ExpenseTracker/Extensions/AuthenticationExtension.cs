using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters{
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            return services;
        }
    }
}