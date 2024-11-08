using AccountService.Domain.Models;
using AccountService.Repository;
using Microsoft.AspNetCore.Identity;

namespace AccountService.ServiceHost.Extensions;

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
        services.AddIdentityCore<UserAccount>(_options)
        .AddRoles<AccountRole>()
        .AddEntityFrameworkStores<DatabaseContext>();
    }
}
