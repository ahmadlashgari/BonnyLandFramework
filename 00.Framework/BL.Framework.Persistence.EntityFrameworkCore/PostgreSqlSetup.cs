using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public static class PostgreSqlSetup
    {
        public static void AddPostgreSqlDbContext<TDbContext>(this IServiceCollection services, string connectionString, string migrationAssembly) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationAssembly))
                       .UseSnakeCaseNamingConvention();
            });
        }
    }
}
