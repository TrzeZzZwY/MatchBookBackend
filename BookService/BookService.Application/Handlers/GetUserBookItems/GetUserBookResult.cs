using BookService.Domain.Common;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetUserBookResult
{
    public required int Id { get; set; }

    public required UserBookItemStatus Status { get; set; }
}
