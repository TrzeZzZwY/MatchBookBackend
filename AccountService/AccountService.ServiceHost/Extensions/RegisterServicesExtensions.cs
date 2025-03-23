using AccountService.Application.Clients;
using AccountService.Application.Services;
using AccountService.ServiceHost.Utils;
using System.Net.Http.Headers;

namespace AccountService.ServiceHost.Extensions;

public static class RegisterServicesExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpContextAccessor();

        var httpConfiguration = configuration.GetSection("HttpClientConfiguration").Get<HttpClientConfiguration>()
            ?? throw new Exception("Cannot get HttpClient options");

        services.AddHttpClient<IBookServiceClient, BookServiceClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = httpConfiguration.BookSeriveUrl;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
    }
}
