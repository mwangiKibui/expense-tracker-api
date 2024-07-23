using ExpenseTracker.Extensions;

namespace ExpenseTracker
{
    public class Startup
    {
        private IConfiguration Configuration {get;}
        private IWebHostEnvironment Environment {get;}

        public Startup(IConfiguration configuration,IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // adding services. (called at run time)
        public void ConfigureServices(IServiceCollection services)
        {
            // authentication.
            services.AddCustomAuthentication(Configuration);
            // core (auto mapper)
            services.AddCoreConfigs();
            // Database
            services.AddDatabaseExtension(Configuration);
            // services
            services.AddServiceExtensions();
            // swagger
            services.AddSwaggerExtension();
        }

        // configuring HTTP request pipeline. Called at run time.
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {endpoints.MapControllers();});
        }
    }
}