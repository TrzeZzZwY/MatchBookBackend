using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookService.Repository.Configurations;

public class UserBookItemConfiguration : IEntityTypeConfiguration<UserBookItem>
{
    public void Configure(EntityTypeBuilder<UserBookItem> builder)
    {
        builder.ToTable(nameof(UserBookItem));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId);
        builder.Property(e => e.Description);
        builder.Property(e => e.Status);
        builder.Property(e => e.Region);
        builder.Property(e => e.CreateDate);
        builder.Property(e => e.UpdateDate);
        builder.HasOne(e => e.BookReference).WithMany(e => e.BookItems);
        builder.HasOne(e => e.BookPoint).WithMany().IsRequired(false);
        builder.HasOne(e => e.ItemImage)
            .WithOne(e => e.UserBookItem)
            .HasForeignKey<UserBookItem>(e => e.ItemImageId)
            .IsRequired(false);
    }
}