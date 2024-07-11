using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchBook.Infrastructure.Data.Configurations.Admin
{
    public class AdminRefreshTokenConfiguration : IEntityTypeConfiguration<AdminRefreshToken>
    {
        public void Configure(EntityTypeBuilder<AdminRefreshToken> builder)
        {
            builder.ToTable(nameof(AdminRefreshToken));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Token);
            builder.Property(e => e.ExpireDate);
            builder.HasOne(e => e.User).WithOne(e => e.RefreshToken).HasForeignKey<AdminRefreshToken>(e => e.UserId);
        }
    }
}
