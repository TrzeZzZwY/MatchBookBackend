namespace AccountService.ServiceHost.Controllers.Dto.RegisterUser;

public record GetManyUserRequest
{
    public required PaginationRequest  PaginationRequest { get; set; }
}
