using BookService.Application.Handlers.GetBookPoint;
using BookService.Domain.Models;

namespace BookService.Application.Extensions;
public static class BookPointExtensions
{
    public static GetBookPointResult ToHandlerResult(this BookPoint item)
    {
        return new GetBookPointResult
        {
            Id = item.Id,
            Lat = item.Lat,
            Long = item.Lat,
            Capacity = item.Capacity,
            Region = item.Region
        };
    }
}
