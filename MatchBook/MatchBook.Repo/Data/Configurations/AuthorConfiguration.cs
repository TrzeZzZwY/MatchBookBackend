using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchBook.Infrastructure.Data.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable(nameof(Author));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name);
        builder.Property(e => e.Surename);
        builder.HasMany(e => e.Books).WithMany(e => e.Authors).UsingEntity("BookAuthorsJoinTable");
        builder.HasMany(e => e.Followers).WithMany(e => e.FollowedAuthors).UsingEntity("UserFollowedAuthorsJoinTable");
    }
}
