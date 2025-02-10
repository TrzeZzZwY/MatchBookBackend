using Microsoft.EntityFrameworkCore;
using ReportingService.Domain.Models;

namespace BookService.Repository;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    public DbSet<CaseEntity> Cases => Set<CaseEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("MB");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
