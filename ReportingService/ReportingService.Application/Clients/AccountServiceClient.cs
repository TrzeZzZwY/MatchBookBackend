using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using ReportingService.Application.Clients.Dto;
using ReportingService.Domain.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ReportingService.Application.Clients;
public class AccountServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private string? GetToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
    }

    private async Task<Result<bool, Error>> SendPostRequest<T>(string url, T body)
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token))
        {
            return new Error("Authorization token is missing", ErrorReason.Unauthorized);
        }

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return true;

        return new Error($"Failed to change user status", ErrorReason.InternalError);
    }

    public Task<Result<bool, Error>> BanUser(int userId) =>
        SendPostRequest("account-status", new ChangeUserStatusRequest
        {
            UserId = userId,
            Status = AccountStatus.BANED
        });
}
