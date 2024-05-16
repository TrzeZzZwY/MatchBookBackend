using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable(nameof(Message));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.messageData);
            builder.HasOne(e => e.User).WithMany(e => e.Messages);
            builder.HasOne(e => e.Chat).WithMany(e => e.Messages);
        }
    }
}
