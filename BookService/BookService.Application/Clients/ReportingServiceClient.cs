using BookService.Application.Clients.Dto;
using BookService.Domain.Common;
using BookService.Domain.Models;
using CSharpFunctionalExtensions;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using BookService.ServiceHost.Extensions;


namespace BookService.Application.Clients;
public class ReportingServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportingServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<Result<bool, Error>> CreateCase(CreateCaseRequest body)
    {
        if (_httpContextAccessor.HttpContext.User.IsAdmin())
            return false;

        var userId = (int)_httpContextAccessor.HttpContext.User.GetId();
        body.UserId = userId;
        var request = new HttpRequestMessage(HttpMethod.Post, "api/CaseAction")
        {
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return true;

        return new Error($"Failed to delete resource", ErrorReason.InternalError);
    }

    public Task<Result<bool, Error>> CreateAuthorCase(Author author) =>
        CreateCase(
            new CreateCaseRequest
            {
                Type = CaseItemType.AUTHOR,
                ItemId = author.Id,
                CaseFields = new()
                {
                    { nameof(author.FirstName), author.FirstName },
                    { nameof(author.LastName), author.LastName },
                    { nameof(author.Country), author.Country },
                    { nameof(author.YearOfBirth), author.YearOfBirth.ToString() }
                }
            });

    public Task<Result<bool, Error>> CreateBookCase(Book book, List<Author> bookAuthors) =>
    CreateCase(
        new CreateCaseRequest
        {
            Type = CaseItemType.BOOK,
            ItemId = book.Id,
            CaseFields = new()
            {
                    { nameof(book.Title), book.Title },
                    { nameof(bookAuthors), string.Join(", ",bookAuthors.Select(e => $"{e.FirstName} {e.LastName}"))},
            }
        });
}
