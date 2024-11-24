using BookService.Application.Handlers.GetBook;
using BookService.Domain.Common;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetUserBookResult
{
    public required int Id { get; set; }

    public required int UserId { get; set; }

    public required string Description { get; set; }

    public required UserBookItemStatus Status { get; set; }

    public required GetBookResult BookReference { get; set; }
}
