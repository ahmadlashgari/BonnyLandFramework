using BL.Framework.AspNetCore.Options;
using BL.Framework.Behaviours;
using Elasticsearch.Net;
using FluentValidation;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BL.Framework
{
    public static class ApplicationServicesSetup
    {
        public static IServiceCollection AddApplicationServicesCore(this IServiceCollection services, Assembly assembly)
        {
            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

            return services;
        }

        public static IServiceCollection AddMassTransitRabbitMQ(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            var serviceProvider = services.BuildServiceProvider();

            var massTransitConfiguration = configuration.GetSection("MassTransit");

            if (massTransitConfiguration == null)
            {
                throw new ArgumentNullException("MassTransit Configuration not found");
            }

            var rabbitMqConfiguration = massTransitConfiguration.GetSection("RabbitMQ");

            if (rabbitMqConfiguration == null)
            {
                throw new ArgumentNullException("MassTransit:RabbitMQ Configuration not found");
            }

            var rabbitMqEndpoints = rabbitMqConfiguration.GetSection("Endpoints").Get<List<RabbitMqEndpointOption>>() ?? new List<RabbitMqEndpointOption>();

            services.AddMassTransit(busConfig =>
            {
                busConfig.UsingRabbitMq((context, config) =>
                {
                    busConfig.AddConsumers(assembly);

                    config.Host(rabbitMqConfiguration.GetValue<string>("Host"), host =>
                    {
                        host.Username(rabbitMqConfiguration.GetValue<string>("Username"));
                        host.Password(rabbitMqConfiguration.GetValue<string>("Password"));
                        host.PublisherConfirmation = rabbitMqConfiguration.GetValue<bool>("PublisherConfirmation");
                    });

                    config.AutoStart = true;

                    foreach (var item in rabbitMqEndpoints)
                    {
                        config.ReceiveEndpoint(item.QueueName, e =>
                        {
                            if (item.UseMessageRetry)
                            {
                                e.UseMessageRetry(r => r.Interval(item.MessageRetryCount, TimeSpan.FromSeconds(item.MessageRetryIntervalSeconds)));
                            }

                            e.PrefetchCount = item.PrefetchCount;
                            e.Durable = item.Durable;
                            e.ExchangeType = item.ExchangeType;

                            if (item.UseConcurrencyLimit)
                            {
                                e.UseConcurrencyLimit(item.ConcurrentMessageLimit);
                            }

                            if (item.UseCircuitBreaker)
                            {
                                e.UseCircuitBreaker(r => r.Handle(new Type[] {
                                    typeof(InvalidOperationException),
                                    typeof(NullReferenceException)
                                }));
                            }

                            if (item.Consumers != null && item.Consumers.Any())
                            {
                                foreach (var consumer in item.Consumers)
                                {
                                    var consumerAssembly = assembly.GetTypes().SingleOrDefault(e => e.Name == consumer);

                                    e.ConfigureConsumer(context, consumerAssembly);
                                }
                            }
                        });
                    }
                });
            }).AddMassTransitHostedService();

            return services;
        }

        public static IServiceCollection AddMassTransitRabbitMQ(this IServiceCollection services, Action<IServiceCollectionBusConfigurator> busConfig)
        {
            services.AddMassTransit(busConfig).AddMassTransitHostedService();

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = configuration.GetSection("Redis");

            if (redisConfiguration == null)
            {
                throw new ArgumentNullException("Redis Configuration not found");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisConfiguration.GetValue<string>("Host")}:{redisConfiguration.GetValue<string>("Port")}";

                options.InstanceName = redisConfiguration.GetValue<string>("InstanceName");
            });

            return services;
        }

        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbConfiguration = configuration.GetSection("MongoDB");

            if (mongoDbConfiguration == null)
            {
                throw new ArgumentNullException("MongoDB Configuration not found");
            }

            var mongoDbCredentialConfiguration = mongoDbConfiguration.GetSection("Credential");

            if (mongoDbCredentialConfiguration == null)
            {
                throw new ArgumentNullException("MongoDB:Credential Configuration not found");
            }

            services.AddTransient<IMongoClient>((s) => new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(
                    mongoDbConfiguration.GetValue<string>("Host"),
                    mongoDbConfiguration.GetValue<int>("Port")
                ),
                Credential = MongoCredential.CreateCredential(
                    mongoDbCredentialConfiguration.GetValue<string>("DatabaseName"),
                    mongoDbCredentialConfiguration.GetValue<string>("Username"),
                    mongoDbCredentialConfiguration.GetValue<string>("Password")
                ),
                UseTls = mongoDbConfiguration.GetValue<bool>("UseTls"),
                DirectConnection = mongoDbConfiguration.GetValue<bool>("DirectConnection"),
                ApplicationName = "BLFramework",
            }));

            return services;
        }

        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchConfiguration = configuration.GetSection("ElasticSearch");

            var pool = new SingleNodeConnectionPool(new Uri(elasticSearchConfiguration.GetValue<string>("Url")));

            var settings = new ConnectionSettings(pool)
                .ServerCertificateValidationCallback((c, ch, e, t) => true)
                .DefaultIndex(elasticSearchConfiguration.GetValue<string>("DefaultIndex"))
                .MaxRetryTimeout(TimeSpan.FromSeconds(elasticSearchConfiguration.GetValue<int>("MaxRetryTimeout")))
                .RequestTimeout(TimeSpan.FromSeconds(elasticSearchConfiguration.GetValue<int>("RequestTimeout")));

            var client = new ElasticClient(settings);

            services.AddSingleton(client);

            return services;
        }
    }
}
