using BookService.Application.Handlers.GetAuthor;
using BookService.Domain.Models;

namespace BookService.Application.Extensions;
public static class AuthorExtensions
{
    public static GetAuthorResult ToHandlerResult(this Author author)
    {
        return new GetAuthorResult
        {
            Id = author.Id,
            FistName = author.FirstName,
            LastName = author.LastName,
            Country = author.Country,
            YearOfBirth = author.YearOfBirth,
            IsRemoved = author.IsDeleted
        };
    }
}
