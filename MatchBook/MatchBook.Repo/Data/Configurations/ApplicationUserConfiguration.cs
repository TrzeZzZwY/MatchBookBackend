using MatchBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchBook.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable(nameof(ApplicationUser));

            builder.HasOne(e => e.Image).WithOne(e => e.User).HasForeignKey<ApplicationUser>(e => e.ImageId).IsRequired(false);
            builder.HasOne(e => e.Region).WithMany(e => e.Users);
            builder.HasMany(e => e.UserBooks).WithOne(e => e.User);
            builder.HasMany(e => e.FollowedAuthors).WithMany(e => e.Followers).UsingEntity("UserFollowedAuthorsJoinTable");
            builder.HasMany(e => e.FollowedTitles).WithMany(e => e.Followers).UsingEntity("UserFollowedTitlesJoinTable");
            builder.HasMany(e => e.Chats).WithMany(e => e.Members).UsingEntity("ChatUsersJoinTable");
            builder.HasMany(e => e.Messages).WithOne(e => e.User);
        }
    }
}
