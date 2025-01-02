using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookService.Repository.Configurations;
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(nameof(Image));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title).IsRequired(true);
        builder.Property(e => e.ImageExtension).HasMaxLength(10).IsRequired(true);
        builder.Property(e => e.CreateDate).IsRequired(true);
        builder.Property(e => e.ImageType).IsRequired(true);
    }
}