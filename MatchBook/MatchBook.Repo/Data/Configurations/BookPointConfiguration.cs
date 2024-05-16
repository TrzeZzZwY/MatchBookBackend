using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchBook.Infrastructure.Data.Configurations;

public class BookPointConfiguration : IEntityTypeConfiguration<BookPoint>
{
    public void Configure(EntityTypeBuilder<BookPoint> builder)
    {
        builder.ToTable(nameof(BookPoint));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Lat);
        builder.Property(e => e.Long);
        builder.HasOne(e => e.Region).WithMany(e => e.BookPoints).IsRequired(false);
        builder.Property(e => e.Capacity).IsRequired(false);
        builder.Property(e => e.CreateDate);
        builder.Property(e => e.UpdateDate);
        builder.HasMany(e => e.Books).WithOne(e => e.BookPoint);
    }
}
