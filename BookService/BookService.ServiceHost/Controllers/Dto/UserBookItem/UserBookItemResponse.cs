using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto.Book;

namespace BookService.ServiceHost.Controllers.Dto.UserItemBook;

public class UserBookItemResponse
{
    public required int Id { get; set; }

    public required int UserId { get; set; }

    public required UserBookItemStatus Status { get; set; }

    public required string Description { get; set; }

    public int? ImageId { get; set; }

    public required BookResponse BookReference { get; set; }

}
