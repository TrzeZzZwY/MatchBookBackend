using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookService.Repository.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable(nameof(Author));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName);
        builder.Property(e => e.LastName);
        builder.Property(e => e.Country);
        builder.Property(e => e.YearOfBirth);
        builder.HasMany(e => e.AuthorBooks).WithMany(e => e.Authors).UsingEntity("BookAuthorsJoinTable");
    }
}
