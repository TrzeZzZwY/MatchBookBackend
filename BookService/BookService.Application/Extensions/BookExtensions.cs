using BookService.Application.Handlers.GetBook;
using BookService.Domain.Models;

namespace BookService.Application.Extensions;
public static class BookExtensions
{
    public static GetBookResult ToHandlerResult(this Book book)
    {
        return new GetBookResult
        {
            Id = book.Id,
            Title = book.Title,
            Authors = book.Authors?.Select(a => a.ToHandlerResult()).ToList(),
            IsRemoved = book.IsDeleted
        };
    }
}
