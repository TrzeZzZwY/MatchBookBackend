using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReportingService.ServiceHost.Middleware;

public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;

    public JwtValidationMiddleware(RequestDelegate next, HttpClient httpClient)
    {
        _next = next;
        _httpClient = httpClient;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
        {
            await _next(context);
            return;
        }

        var token = authHeader.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token) ||
            !(await IsTokenValid(token)) ||
            !TryExtractClaims(token, out var claimsPrincipal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Please provide valid token.");
            return;
        }
        
        context.User = claimsPrincipal;

        await _next(context);
    }

    public async Task<bool> IsTokenValid(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8900/api/Auth/OnlyValidToken")
        {
            Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return false;
        return true;
    }

    private bool TryExtractClaims(string token, out ClaimsPrincipal? claimsPrincipal)
    {
        claimsPrincipal = null;
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return true;
        }
        catch
        {
            return false;
        }

    }
}
