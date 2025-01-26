using AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Repository.Configurations;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken));

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.AccountId);
        builder.Property(e => e.Token);
        builder.Property(e => e.ExpireDate);

        builder.HasOne(e => e.Account).WithOne(e => e.RefreshToken).HasForeignKey<RefreshToken>(e => e.AccountId);
    }
}
