using MatchBook.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations.Admin
{
    public class ApplicationAdminConfiguration : IEntityTypeConfiguration<ApplicationAdmin>
    {
        public void Configure(EntityTypeBuilder<ApplicationAdmin> builder)
        {
            builder.ToTable(nameof(ApplicationAdmin));

            builder.HasOne(e => e.RefreshToken).WithOne(e => e.User).HasForeignKey<ApplicationAdmin>(e => e.RefreshTokenId).IsRequired(false);
        }
    }
}
