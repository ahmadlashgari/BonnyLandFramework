using BL.Framework.AspNetCore.Security;
using BL.Framework.Persistence.EntityFrameworkCore;
using BL.Framework.Persistence.EntityFrameworkCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Framework
{
    public static class InfrastructureServicesSetup
    {
        public static IServiceCollection AddInfrastructureServicesCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddTransient(typeof(IRepository<>), typeof(EFRepository<,>));
            services.AddTransient(typeof(IReadRepository<>), typeof(EFReadRepository<,>));

            return services;
        }
    }
}
