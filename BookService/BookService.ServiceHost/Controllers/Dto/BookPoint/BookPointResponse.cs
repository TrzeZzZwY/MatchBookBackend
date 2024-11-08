using BookService.Domain.Common;

namespace BookService.ServiceHost.Controllers.Dto.BookPoint;

public class BookPointResponse
{
    public required int Id { get; set; }

    public required int Lat { get; set; }

    public required int Long { get; set; }

    public required Region Region { get; set; }

    public int? Capacity { get; set; }
}
