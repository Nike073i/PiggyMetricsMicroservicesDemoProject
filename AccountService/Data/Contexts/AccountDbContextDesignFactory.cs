using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AccountService.Data.Contexts
{
    public class AccountDbContextDesignFactory : IDesignTimeDbContextFactory<AccountDbContext>
    {
        private const string ConnectionString =
            "Host=localhost;Port=6446;User ID=root;Password=example;Database=account_db";

        public AccountDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountDbContext>();
            optionsBuilder.UseNpgsql(ConnectionString);

            return new AccountDbContext(optionsBuilder.Options);
        }
    }
}
