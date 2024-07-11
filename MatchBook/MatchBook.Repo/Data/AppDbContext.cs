using MatchBook.Domain.Models;
using MatchBook.Domain.Models.Identity;
using MatchBook.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchBook.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<BookPoint> BookPoints => Set<BookPoint>();

    public DbSet<BookTitle> BookTitles => Set<BookTitle>();

    public DbSet<Chat> Chats => Set<Chat>();

    public DbSet<Image> Images => Set<Image>();

    public DbSet<ImageTarget> ImageTargets => Set<ImageTarget>();

    public DbSet<Message> Messages => Set<Message>();

    public DbSet<Region> Regions => Set<Region>();

    public DbSet<Report> Reports => Set<Report>();

    public DbSet<RefreshToken> Tokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("user");
        modelBuilder
            .ApplyConfiguration(new ApplicationRoleConfiguration())
            .ApplyConfiguration(new ApplicationUserConfiguration())
            .ApplyConfiguration(new AuthorConfiguration())
            .ApplyConfiguration(new BookConfiguration())
            .ApplyConfiguration(new BookPointConfiguration())
            .ApplyConfiguration(new BookTitleConfiguration())
            .ApplyConfiguration(new ChatConfiguration())
            .ApplyConfiguration(new ImageConfiguration())
            .ApplyConfiguration(new ImageTargetConfiguration())
            .ApplyConfiguration(new MessageConfiguration())
            .ApplyConfiguration(new RefreshTokenConfiguration())
            .ApplyConfiguration(new RegionConfiguration());
    }
}
