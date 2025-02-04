using BookService.Domain.Common;

namespace BookService.ServiceHost.Controllers.Dto.UserItemBook;

public class UserBookItemRequest
{
    public required string Description { get; set; }

    public required UserBookItemStatus Status { get; set; }
     
    public required int BookReferenceId { get; set; }

    public int? BookPointId { get; set; }

    public int? ImageId { get; set; }
}
