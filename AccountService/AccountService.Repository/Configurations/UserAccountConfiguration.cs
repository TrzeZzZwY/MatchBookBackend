using AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repository.Configurations;
public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable(nameof(UserAccount));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName);
        builder.Property(e => e.LastName);
        builder.Property(e => e.Region);
        builder.Property(e => e.BirthDate);
        builder.HasOne(e => e.Account).WithOne(e => e.UserAccount)
            .HasForeignKey<Account>(e => e.UserAccountId);
    }
}