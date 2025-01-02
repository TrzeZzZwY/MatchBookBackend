using BookService.Application.Handlers.GetUserBookItems;
using BookService.Domain.Common;

namespace BookService.Application.Handlers.GetUserLikes;
public class GetUserLikesResult
{
    public required int UserId { get; set; }

    public required PaginatedResult<GetUserBookResult> Items { get; set; }
}
