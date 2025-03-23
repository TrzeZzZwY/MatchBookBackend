using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace AccountService.Application.Clients;
public class BookServiceClient : IBookServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private string? GetToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
    }

    public async Task<Result<bool, Error>> UpdateItemsRegion()
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token))
        {
            return new Error("Authorization token is missing", ErrorReason.Unauthorized);
        }

        var request = new HttpRequestMessage(HttpMethod.Put, "api/UserBookItem/batch-change-region");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return true;

        return new Error($"Failed to change items region", ErrorReason.InternalError);
    }
}
