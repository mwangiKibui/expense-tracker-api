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
            services.AddDatabaseExtension(Configuration,Environment);
            // services
            services.AddServiceExtensions();
            // swagger
            services.AddSwaggerExtension();
        }

        // configuring HTTP request pipeline. Called at run time.
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            // allow swagger ui on all environments for now
            // if (env.IsDevelopment())
            // {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTracker API"); });
            // }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {endpoints.MapControllers();});
        }
    }
}