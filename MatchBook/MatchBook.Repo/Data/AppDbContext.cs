using MatchBook.Domain.Models;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applying all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
