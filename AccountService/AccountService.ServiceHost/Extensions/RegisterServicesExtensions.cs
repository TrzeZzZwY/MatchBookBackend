using AccountService.Application.Services;

namespace AccountService.ServiceHost.Extensions;

public static class RegisterServicesExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
    }
}
