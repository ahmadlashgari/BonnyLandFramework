using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public static class SqlServerSetup
    {
        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, string connectionString, string migrationAssembly) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
            });
        }
    }
}
