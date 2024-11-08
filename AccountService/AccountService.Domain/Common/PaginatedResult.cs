namespace AccountService.Domain.Common;

public class PaginatedResult<TItem>
{
    public required int PageNumber { get; init; }

    public required int PageSize { get; init; }

    public required int ItemCount { get; init; }

    public required List<TItem> items { get; init; }
}
