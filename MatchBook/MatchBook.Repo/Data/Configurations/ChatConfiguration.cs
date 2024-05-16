using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable(nameof(Chat));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Topic);
            builder.HasMany(e => e.Members).WithMany(e => e.Chats).UsingEntity("ChatUsersJoinTable");
            builder.HasMany(e => e.Messages).WithOne(e => e.Chat);
        }
    }
}
