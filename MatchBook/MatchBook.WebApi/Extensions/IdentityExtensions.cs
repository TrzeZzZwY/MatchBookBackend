using MatchBook.Domain.Models;
using MatchBook.Domain.Models.Identity;
using MatchBook.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace MatchBook.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        private static Action<IdentityOptions> _options = options =>
        {
            options.User.AllowedUserNameCharacters = string.Empty;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 3;
        };
        public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationUser>(_options)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

            services.AddIdentityCore<ApplicationAdmin>(_options)
            .AddRoles<ApplicationAdminRole>()
            .AddEntityFrameworkStores<AdminDbContext>()
            .AddApiEndpoints();
        }
    }
}
