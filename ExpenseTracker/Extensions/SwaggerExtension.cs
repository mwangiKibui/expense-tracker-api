using Microsoft.OpenApi.Models;

namespace ExpenseTracker.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                c => {
                    var securityScheme = new OpenApiSecurityScheme{
                        Name = "JWT Authentication",
                        Description = "Enter your JWT Token in this field",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                    };
                    c.AddSecurityDefinition("Bearer", securityScheme);
                    var securityRequirement = new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {} // no specific scope is needed
                        }
                    };
                    c.AddSecurityRequirement(securityRequirement);
                }
            );
            services.AddRouting(options => options.LowercaseUrls = true);
            return services;
        }
    }
}