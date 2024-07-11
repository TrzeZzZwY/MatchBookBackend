using MatchBook.Domain.Models;
using MatchBook.Domain.Models.Identity;
using MatchBook.Infrastructure.Data.Configurations;
using MatchBook.Infrastructure.Data.Configurations.Admin;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchBook.Infrastructure.Data;

public class AdminDbContext : IdentityDbContext<ApplicationAdmin, ApplicationAdminRole, int>
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("admin");
        modelBuilder.ApplyConfiguration(new AdminRefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationAdminConfiguration());
    }
}
