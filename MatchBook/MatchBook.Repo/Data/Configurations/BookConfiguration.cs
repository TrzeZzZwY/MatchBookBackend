using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable(nameof(Book));

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.User).WithMany(e => e.UserBooks);
            builder.HasOne(e => e.Title).WithMany(e => e.Books);
            builder.Property(e => e.Category);
            builder.HasOne(e => e.BookPoint).WithMany(e => e.Books);
            builder.Property(e => e.CreateDate);
            builder.Property(e => e.UpdateDate);
            builder.Property(e => e.Description);
            builder.HasOne(e => e.Image).WithOne(e => e.Book).HasForeignKey<Book>(e => e.ImageId).IsRequired(false);
            builder.Property(e => e.Views);
            builder.Property(e => e.Visibility);
            builder.HasMany(e => e.Authors).WithMany(e => e.Books).UsingEntity("BookAuthorsJoinTable");
            builder.HasMany(e => e.Likes).WithMany(e => e.Likes).UsingEntity("UserBookLikesJoinTable",
                l => l.HasOne(typeof(ApplicationUser)).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.NoAction),
                r => r.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.NoAction)
                );
        }
    }
}
