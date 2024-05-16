using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class BookTitleConfiguration : IEntityTypeConfiguration<BookTitle>
    {
        public void Configure(EntityTypeBuilder<BookTitle> builder)
        {
            builder.ToTable(nameof(BookTitle));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title);
            builder.HasMany(e => e.Books).WithOne(e => e.Title);
            builder.HasMany(e => e.Followers).WithMany(e => e.FollowedTitles).UsingEntity("UserFollowedTitlesJoinTable");
        }
    }
}
