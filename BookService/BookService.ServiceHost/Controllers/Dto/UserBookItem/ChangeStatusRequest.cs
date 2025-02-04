using BookService.Domain.Common;

namespace BookService.ServiceHost.Controllers.Dto.UserBookItem;

public class ChangeStatusRequest
{
    public required UserBookItemStatus Status { get; set; }
}
