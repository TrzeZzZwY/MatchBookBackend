using BookService.Application.Handlers.GetUserBookItems;
using BookService.Domain.Models;

namespace BookService.Application.Extensions;
public static class UserBookItemsExtensions
{
    public static GetUserBookResult ToHandlerResult(this UserBookItem item)
    {
        return new GetUserBookResult
        {
            Id = item.Id,
            Status = item.Status,
            Description = item.Description,
            UserId = item.UserId,
            BookReference = item.BookReference.ToHandlerResult()
        };
    }
}
