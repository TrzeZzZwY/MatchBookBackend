using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable(nameof(Region));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.RegionName);
            builder.Property(e => e.IsEnabled);
            builder.Property(e => e.CreateDate);
            builder.Property(e => e.UpdateDate);
            builder.HasMany(e => e.BookPoints).WithOne(e => e.Region);
            builder.HasMany(e => e.Users).WithOne(e => e.Region);
        }
    }
}
