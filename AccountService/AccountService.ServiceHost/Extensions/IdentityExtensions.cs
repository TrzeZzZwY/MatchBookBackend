using AccountService.Domain.Models;
using AccountService.Repository;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
        options.SignIn.RequireConfirmedEmail = false;
    };
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<Account>(_options)
        .AddRoles<AccountRole>()
        .AddEntityFrameworkStores<DatabaseContext>();
    }

    public static int? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        var value = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (value is null) return null;
        return int.Parse(value);
    }
}
