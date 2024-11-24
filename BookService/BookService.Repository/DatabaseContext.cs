using BookService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.Repository;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<BookPoint> BookPoints => Set<BookPoint>();

    public DbSet<UserBookItem> UserBookItems => Set<UserBookItem>();

    public DbSet<UserLikesBooks> UserLikesBooks => Set<UserLikesBooks>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("MB");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
