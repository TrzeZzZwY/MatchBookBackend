namespace BookService.Domain.Common;
public class PaginatedResult<T>
{
    public required List<T> Items { get; set; }

    public required int PageSize { get; set; }

    public required int PageNumber { get; set; }

    public required int TotalItemsCount { get; set; }
}
