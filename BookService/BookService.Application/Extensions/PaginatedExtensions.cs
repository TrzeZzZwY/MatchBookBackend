using BookService.Domain.Common;

namespace BookService.Application.Extensions;
public static class PaginatedExtensions
{
    public static PaginatedResult<T> ToPaginatedResult<T>(this PaginationOptions paginationOptions, List<T> items, int totalItemsCount)
    {
        return new PaginatedResult<T>
        {
            Items = items,
            PageNumber = paginationOptions.PageNumber,
            PageSize = paginationOptions.PageSize,
            TotalItemsCount = totalItemsCount
        };
    }
}
