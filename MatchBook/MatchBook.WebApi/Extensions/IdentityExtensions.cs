using MatchBook.Domain.Models;
using MatchBook.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace MatchBook.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.AllowedUserNameCharacters = string.Empty;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
        }
    }
}
