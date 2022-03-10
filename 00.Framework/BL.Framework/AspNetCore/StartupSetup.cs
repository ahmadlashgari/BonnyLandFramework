using BL.Framework.AspNetCore.Filters;
using BL.Framework.IdentityServer;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Framework.AspNetCore
{
    public static class StartupSetup
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(config =>
            {
                config.AddDefaultPolicy(options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            return services;
        }

        public static IServiceCollection AddIdentityServerCore<TUser, TDbContext>(this IServiceCollection services, IConfiguration configuration, Assembly migrationsAssembly) where TUser : IdentityUser where TDbContext : ApiAuthorizationDbContext<TUser>
        {
            services.AddDefaultIdentity<TUser>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

            var identityServerConfiguration = configuration.GetSection("IdentityServer");

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = identityServerConfiguration.GetValue<string>("IssuerUri");
                options.Events.RaiseErrorEvents = identityServerConfiguration.GetValue<bool>("RaiseErrorEvents");
                options.Events.RaiseInformationEvents = identityServerConfiguration.GetValue<bool>("RaiseInformationEvents");
                options.Events.RaiseFailureEvents = identityServerConfiguration.GetValue<bool>("RaiseFailureEvents");
                options.Events.RaiseSuccessEvents = identityServerConfiguration.GetValue<bool>("RaiseSuccessEvents");
            })
                .AddConfigurationStore(options =>
                {
                    options.DefaultSchema = identityServerConfiguration.GetValue<string>("DefaultSchema");

                    var databaseProvider = identityServerConfiguration.GetValue<string>("Provider");
                    var connectionString = configuration.GetConnectionString("IdentityServer");

                    switch (databaseProvider)
                    {
                        case "PostgreSQL":
                            options.ConfigureDbContext = db => db.UseNpgsql(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly.FullName)).UseSnakeCaseNamingConvention();

                            break;

                        case "SqlServer":
                            options.ConfigureDbContext = db => db.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly.FullName));

                            break;
                        default:
                            throw new ArgumentNullException("Identity Server Provider Not Found");
                    }
                })
                .AddOperationalStore(options =>
                {
                    options.DefaultSchema = identityServerConfiguration.GetValue<string>("DefaultSchema");

                    var databaseProvider = identityServerConfiguration.GetValue<string>("Provider");
                    var connectionString = configuration.GetConnectionString("IdentityServer");

                    switch (databaseProvider)
                    {
                        case "PostgreSQL":
                            options.ConfigureDbContext = db => db.UseNpgsql(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly.FullName)).UseSnakeCaseNamingConvention();

                            break;

                        case "SqlServer":
                            options.ConfigureDbContext = db => db.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly.FullName));

                            break;
                        default:
                            throw new ArgumentNullException("Identity Server Provider Not Found");
                    }
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<TUser>()
                .AddProfileService<IdProfileService>();

            return services;
        }

        public static IServiceCollection AddIdentityServerApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityConfiguration = configuration.GetSection("IdentityServer");

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityConfiguration.GetValue<string>("AuthorityUrl");
                    options.ApiName = identityConfiguration.GetValue<string>("ApiName");
                    options.RequireHttpsMetadata = identityConfiguration.GetValue<bool>("RequireHttpsMetadata");

                    options.JwtBackChannelHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });

            return services;
        }

        public static IServiceCollection AddIdentityServerJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityConfiguration = configuration.GetSection("IdentityServer");

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityConfiguration.GetValue<string>("AuthorityUrl");
                    options.RequireHttpsMetadata = identityConfiguration.GetValue<bool>("RequireHttpsMetadata");

                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddMvcBase(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            //.AddLocalization()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            })
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static IHealthChecksBuilder AddHealthChecksCore(this IServiceCollection services)
        {
            return services.AddHealthChecks();
        }

        public static IHealthChecksBuilder AddPosgtreSqlHealthChecks(this IHealthChecksBuilder services, IConfiguration configuration)
        {
            services.AddNpgSql(configuration.GetConnectionString("PostgreSqlHealthChecksConnection"));

            return services;
        }

        public static IServiceCollection AddLocaleServices(this IServiceCollection services, IConfiguration configuration)
        {
            var localeConfiguration = configuration.GetSection("Locale");

            if (localeConfiguration == null)
            {
                throw new ArgumentNullException("Locale Configuration Not Found");
            }

            var defaultCulture = localeConfiguration.GetValue<string>("DefaultCulture");

            if (defaultCulture == null)
            {
                throw new ArgumentNullException("Locale DefaultCulture Not Found");
            }

            var resourcesPath = localeConfiguration.GetValue<string>("ResourcesPath");

            if (resourcesPath == null)
            {
                throw new ArgumentNullException("Locale ResourcesPath Not Found");
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(defaultCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(defaultCulture);

            services.AddLocalization(options => options.ResourcesPath = resourcesPath);

            return services;
        }

        public static IServiceCollection AddOcelotGateway(this IServiceCollection services, IConfiguration configuration)
        {
            var ocelotGatewayConfiguration = configuration.GetSection("Ocelot");

            services.AddOcelot(ocelotGatewayConfiguration);

            return services;
        }

        public static IMvcBuilder AddLocalization(this IMvcBuilder builder)
        {
            builder.AddMvcLocalization()
                   .AddViewLocalization()
                   .AddDataAnnotationsLocalization();

            return builder;
        }

        public static IApplicationBuilder UseExceptionHandlers(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            return app;
        }

        public static IApplicationBuilder UseAppCore(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static IApplicationBuilder UseLocaleConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            var localeConfiguration = configuration.GetSection("Locale");

            if (localeConfiguration == null)
            {
                throw new ArgumentNullException("Locale Configuration Not Found");
            }

            var localeSupportedCultures = localeConfiguration.GetSection("SupportedCultures").Get<List<string>>();

            if (localeSupportedCultures == null)
            {
                throw new ArgumentNullException("Locale SupportedCultures Not Found");
            }

            var localeSupportedUICultures = localeConfiguration.GetSection("SupportedUICultures").Get<List<string>>();

            if (localeSupportedUICultures == null)
            {
                throw new ArgumentNullException("Locale SupportedUICultures Not Found");
            }

            var supportedCultures = new List<CultureInfo>();
            var supportedUICultures = new List<CultureInfo>();

            foreach (var item in localeSupportedCultures)
            {
                supportedCultures.Add(new CultureInfo(item));
            }

            foreach (var item in localeSupportedUICultures)
            {
                supportedUICultures.Add(new CultureInfo(item));
            }

            var defaultCulture = localeConfiguration.GetValue<string>("DefaultCulture");

            if (defaultCulture == null)
            {
                throw new ArgumentNullException("Locale DefaultCulture Not Found");
            }

            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedUICultures;
            });

            return app;
        }

        public static IApplicationBuilder UseOcelotGateway(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseOcelot();

            return app;
        }

        public static IApplicationBuilder UseEndPoints(this IApplicationBuilder app, bool healthChecksEndPoint, bool swaggerEndPoint)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                if (healthChecksEndPoint)
                {
                    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                }

                if (swaggerEndPoint)
                {
                    endpoints.MapGet("/", async context => await Task.Run(() => context.Response.Redirect("/api-docs")));
                }
            });

            return app;
        }
    }
}