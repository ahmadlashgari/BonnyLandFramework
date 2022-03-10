using BL.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BL.Test1.ApplicationServices
{
    public static class Setup
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServicesCore(Assembly.GetExecutingAssembly());
            services.AddMassTransitRabbitMQ(configuration, Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
