using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class ImageTargetConfiguration : IEntityTypeConfiguration<ImageTarget>
    {
        public void Configure(EntityTypeBuilder<ImageTarget> builder)
        {
            builder.ToTable(nameof(ImageTarget));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Type);
            builder.Property(e => e.Path);
        }
    }
}
