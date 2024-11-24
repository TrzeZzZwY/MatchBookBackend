using BookService.Application.Handlers.GetUserBookItems;

namespace BookService.Application.Handlers.GetUserLikes;
public class GetUserLikesResult
{
    public required int UserId { get; set; }

    public required List<GetUserBookResult> Items { get; set; }
}
