using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Services;
using ExpenseTracker.Mappings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters{
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddAuthentication();
// Add services to the container.
builder.Services.AddControllers();
// add the db connection.
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("ConnectionString"))
);
// builder.Services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
builder.Services.AddAutoMapper(typeof(ExpenseTypeProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
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
builder.Services.AddRouting(options => options.LowercaseUrls = true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
