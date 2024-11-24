using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookService.Repository.Configurations;

public class UserLikeBookConfiguration : IEntityTypeConfiguration<UserLikesBooks>
{
    public void Configure(EntityTypeBuilder<UserLikesBooks> builder)
    {
        builder.HasKey(e => new { e.UserId, e.UserBookItemId});

        builder.HasOne(e => e.userBookItem).WithMany(e => e.UserLikes);
    }
}
