using BookService.Domain.Common;

namespace BookService.Application.Handlers.GetBookPoint;
public class GetBookPointResult
{
    public required int Id { get; set; }

    public required int Lat { get; set; }

    public required int Long { get; set; }

    public required Region Region { get; set; }

    public int? Capacity { get; set; }
}
