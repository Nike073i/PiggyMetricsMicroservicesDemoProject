using System.Threading.Tasks;
using AccountService.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccountService.Extensions
{
    public static class DatabaseMigrationExtensions
    {
        public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<AccountDbContext>();
            await dbContext.Database.MigrateAsync();
            return host;
        }
    }
}
