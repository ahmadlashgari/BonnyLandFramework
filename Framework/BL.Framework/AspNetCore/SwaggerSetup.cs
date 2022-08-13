using BL.Framework.AspNetCore.Options;
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
            var swaggerConfiguration = configuration.GetSection("Swagger").Get<SwaggerOption>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(swaggerConfiguration.SecurityDefinitionName, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(swaggerConfiguration.AuthorizationUrl, UriKind.RelativeOrAbsolute),
                            TokenUrl = new Uri(swaggerConfiguration.TokenUrl, UriKind.RelativeOrAbsolute),
                            Scopes = swaggerConfiguration.OAuthScopes
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
                               Id = swaggerConfiguration.SecurityDefinitionName
                           }
                       },
                       new[] { string.Join(" ", swaggerConfiguration.OAuthScopes.Values) }
                    }
                });

                c.EnableAnnotations();

                c.SwaggerDoc(swaggerConfiguration.DocVersion, new OpenApiInfo
                {
                    Version = swaggerConfiguration.DocVersion,
                    Title = swaggerConfiguration.DocTitle,
                    Description = swaggerConfiguration.DocDescription,
                    TermsOfService = new Uri(swaggerConfiguration.DocTermsOfService),
                    Contact = new OpenApiContact
                    {
                        Name = swaggerConfiguration.DocContactName,
                        Email = swaggerConfiguration.DocContactEmail
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
                c.DocumentTitle = swaggerConfiguration.GetValue<string>("DocumentTitle");
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
