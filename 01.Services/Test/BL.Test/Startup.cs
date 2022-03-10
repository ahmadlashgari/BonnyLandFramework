using BL.Framework.AspNetCore;
using BL.Test.ApplicationServices;
using BL.Test.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Test
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsPolicy();

            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

            // Identity
            services.AddIdentityServerApiAuthentication(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorPrivilege", policy =>
                {
                    policy.RequireRole("Administrator");
                });

                options.AddPolicy("ContentProducerPrivilege", policy =>
                {
                    policy.RequireRole("ContentProducer", "Administrator");
                });
            });

            // Swagger
            services.AddSwaggerServices(Configuration);

            // Mvc
            services.AddMvcBase().AddLocalization();

            // Health Checks
            services.AddHealthChecksCore().AddPosgtreSqlHealthChecks(Configuration);

            // Core Services
            services.AddApplicationServices(Configuration);
            services.AddInfrastructure();
            services.AddLocaleServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandlers(env);

            // Swagger
            app.UseSwaggerConfiguration(Configuration);

            app.UseAppCore().UseLocaleConfiguration(Configuration);

            app.UseEndPoints(true, true);
        }
    }
}
