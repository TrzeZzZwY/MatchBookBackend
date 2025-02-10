using AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repository.Configurations;
public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable(nameof(AdminAccount));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName);
        builder.Property(e => e.LastName);
        builder.HasOne(e => e.AccountCreator).WithOne();
        builder.HasOne(e => e.Account).WithOne(e => e.AdminAccount)
            .HasForeignKey<Account>(e  => e.AdminAccountId);
    }
}