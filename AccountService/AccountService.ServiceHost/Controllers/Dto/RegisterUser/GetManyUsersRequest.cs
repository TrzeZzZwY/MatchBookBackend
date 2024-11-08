namespace AccountService.ServiceHost.Controllers.Dto.RegisterUser;

public record GetManyUsersRequest
{
    public required PaginationRequest  PaginationRequest { get; set; }
}
