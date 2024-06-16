using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchBook.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(nameof(RefreshToken));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Token);
            builder.Property(e => e.ExpireDate);
            builder.HasOne(e => e.User).WithOne(e => e.RefreshToken).HasForeignKey<RefreshToken>(e => e.UserId);
        }
    }
}
