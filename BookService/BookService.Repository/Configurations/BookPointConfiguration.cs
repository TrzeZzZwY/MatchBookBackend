using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookService.Repository.Configurations;

public class BookPointConfiguration : IEntityTypeConfiguration<BookPoint>
{
    public void Configure(EntityTypeBuilder<BookPoint> builder)
    {
        builder.ToTable(nameof(BookPoint));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Lat);
        builder.Property(e => e.Long);
        builder.Property(e => e.Capacity).IsRequired(false);
        builder.Property(e => e.CreateDate);
        builder.Property(e => e.UpdateDate);
    }
}
