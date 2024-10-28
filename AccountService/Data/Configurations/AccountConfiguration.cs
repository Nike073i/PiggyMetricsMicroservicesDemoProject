using System;
using AccountService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(e => e.Name);
            builder.HasMany(e => e.Incomes).WithOne().HasForeignKey(i => i.AccountName);
            builder.HasMany(e => e.Expenses).WithOne().HasForeignKey(i => i.AccountName);
        }
    }
}
