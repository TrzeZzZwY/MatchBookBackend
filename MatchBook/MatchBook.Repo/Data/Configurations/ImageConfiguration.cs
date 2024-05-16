using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable(nameof(Image));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title);
            builder.Property(e => e.Slug);
            builder.HasOne(e => e.Target).WithOne(e => e.Image).HasForeignKey<Image>(e => e.TargetId); //TODO: relationship with ImageTarget  
        }
    }
}
