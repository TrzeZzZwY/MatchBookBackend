namespace BookService.ServiceHost.Controllers.Dto;

public class PaginationWrapper<TItem>
{
    public PaginationWrapper()
    {
        
    }

    public required int ItemsCount { get; set; }

    public required int TotalItemsCount { get; set; }

    public required int PageNumber { get; set; }

    public required int PageSize { get; set; }

    public required IEnumerable<TItem> Items { get; set; }
}
