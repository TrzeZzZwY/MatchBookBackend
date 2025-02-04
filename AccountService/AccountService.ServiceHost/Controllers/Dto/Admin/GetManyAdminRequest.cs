namespace AccountService.ServiceHost.Controllers.Dto.Admin;

public record GetManyAdminRequest
{
    public required PaginationRequest  PaginationRequest { get; set; }
}
