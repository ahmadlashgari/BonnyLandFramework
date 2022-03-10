using BL.Framework.Persistence.EntityFrameworkCore;
using BL.Framework.Persistence.MongoDB.Interfaces;
using BL.Test.ApplicationServices.Common.Interfaces;
using BL.Test.Infrastructure.Persistence;
using BL.Test.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BL.Test.Infrastructure
{
    public static class Setup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddPostgreSqlDbContext<TestDbContext>(connectionString,
                typeof(Setup).GetTypeInfo().Assembly.GetName().Name);

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(MongoRepositoryBase<>));
            services.AddTransient(typeof(IReadRepository<>), typeof(MongoReadRepositoryBase<>));

            services.AddTransient<IBookService, BookService>();

            return services;
        }
    }
}
