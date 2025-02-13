using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookService.Repository.Configurations;

public class BookExchangeConfiguration : IEntityTypeConfiguration<BookExchange>
{
    public void Configure(EntityTypeBuilder<BookExchange> builder)
    {
        builder.ToTable(nameof(BookExchange));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.InitiatorUserId);
        builder.Property(e => e.InitiatorBookItemId);
        builder.Property(e => e.ReceiverUserId);
        builder.Property(e => e.ReceiverBookItemId);
        builder.Property(e => e.Status);     
    }
}