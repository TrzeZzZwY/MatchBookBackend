using BookService.Application.Clients.Dto;
using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using System.Text.Json;

namespace BookService.Application.Clients;
public class AccountServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public AccountServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<GetUserResponse, Error>> FetchUser(int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/User/{userId}");

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var rawContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetUserResponse>(rawContent, options);
        }
        return new Error($"Failed to delete resource", ErrorReason.InternalError);
    }
}
