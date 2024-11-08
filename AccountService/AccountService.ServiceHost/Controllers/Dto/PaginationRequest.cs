namespace AccountService.ServiceHost.Controllers.Dto;

public record PaginationRequest
{
    public required int PageSize { get; set; }

    public required int PageNumber{ get; set; }
}
