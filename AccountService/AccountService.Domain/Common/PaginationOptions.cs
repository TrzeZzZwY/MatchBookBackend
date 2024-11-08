namespace AccountService.Domain.Common;

public class PaginationOptions
{
    public required int PageNumber { get; init; }

    public required int PageSize { get; init; }
}
