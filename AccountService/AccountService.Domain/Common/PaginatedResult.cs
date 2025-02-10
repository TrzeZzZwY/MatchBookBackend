namespace AccountService.Domain.Common;

public class PaginatedResult<TItem>
{
    public required int PageNumber { get; init; }

    public required int PageSize { get; init; }

    public required int TotalItemsCount { get; init; }

    public required List<TItem> Items { get; init; }
}
