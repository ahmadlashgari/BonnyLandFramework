using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace BL.Framework.AspNetCore
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerConfiguration = configuration.GetSection("Swagger");

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(swaggerConfiguration.GetValue<string>("SecurityDefinitionName"), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(swaggerConfiguration.GetValue<string>("AuthorizationUrl"), UriKind.RelativeOrAbsolute),
                            TokenUrl = new Uri(swaggerConfiguration.GetValue<string>("TokenUrl"), UriKind.RelativeOrAbsolute),
                            Scopes = new Dictionary<string, string>
                            {
                                { "read", "Read Access" },
                                { "write", "Write Access" },
                                { "delete", "Delete Access" },
                                { "openid", "OpenId Access" },
                                { "profile", "Profile Access" }
                            }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = swaggerConfiguration.GetValue<string>("SecurityDefinitionName")
                            }
                        },
                        new[] { "read write delete" }
                    }
                });

                c.EnableAnnotations();

                c.SwaggerDoc(swaggerConfiguration.GetValue<string>("DocVersion"), new OpenApiInfo
                {
                    Version = swaggerConfiguration.GetValue<string>("DocVersion"),
                    Title = swaggerConfiguration.GetValue<string>("DocTitle"),
                    Description = swaggerConfiguration.GetValue<string>("DocDescription"),
                    TermsOfService = new Uri(swaggerConfiguration.GetValue<string>("DocTermsOfService")),
                    Contact = new OpenApiContact
                    {
                        Name = swaggerConfiguration.GetValue<string>("DocContactName"),
                        Email = swaggerConfiguration.GetValue<string>("DocContactEmail")
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerConfiguration = configuration.GetSection("Swagger");

            app.UseSwagger(c =>
            {
                c.RouteTemplate = swaggerConfiguration.GetValue<string>("RouteTemplate");
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerConfiguration.GetValue<string>("EndpointUrl"), swaggerConfiguration.GetValue<string>("EndpointTitle"));
                c.RoutePrefix = swaggerConfiguration.GetValue<string>("RoutePrefix");

                c.OAuthClientId(swaggerConfiguration.GetValue<string>("OAuthClientId"));
                c.OAuthClientSecret(swaggerConfiguration.GetValue<string>("OAuthClientSecret"));
                c.OAuthUsePkce();
            });

            return app;
        }
    }
}
