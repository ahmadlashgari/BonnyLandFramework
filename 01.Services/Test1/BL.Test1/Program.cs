using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace BL.Test1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = Framework.AspNetCore.HostBuilder.CreateHostBuilder<Startup>(args, true, true).Build();

            using var scope = app.Services.CreateScope();

            //var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            //await dbContext.Database.MigrateAsync();

            await app.RunAsync();
        }
    }
}
