namespace AccountService.ServiceHost.Controllers.Dto.Admin;

public class LoginAdminRequest
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}
