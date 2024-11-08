using BookService.Domain.Common;

namespace BookService.ServiceHost.Controllers.Dto.UserItemBook;

public class UserBookItemResponse
{
    public required int Id { get; set; }

    public required UserBookItemStatus Status { get; set; }
}
