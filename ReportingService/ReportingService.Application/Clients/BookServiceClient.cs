using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using ReportingService.Domain.Common;
using System.Net.Http.Headers;

namespace ReportingService.Application.Clients;
public class BookServiceClient
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

    private async Task<Result<bool, Error>> SendDeleteRequest(string url)
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token))
        {
            return new Error("Authorization token is missing", ErrorReason.Unauthorized);
        }

        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return true;

        return new Error($"Failed to delete resource", ErrorReason.InternalError);
    }

    public Task<Result<bool, Error>> DeleteBook(int bookId) =>
        SendDeleteRequest($"api/Book/{bookId}");

    public Task<Result<bool, Error>> DeleteAuthor(int authorId) =>
        SendDeleteRequest($"api/Author/{authorId}");

    public Task<Result<bool, Error>> DeleteUserItem(int userItemId) =>
        SendDeleteRequest($"api/UserBookItem/{userItemId}");
}
