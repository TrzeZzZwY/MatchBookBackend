namespace AccountService.ServiceHost.Controllers.Dto.Auth;

public class RefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}