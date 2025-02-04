namespace AccountService.ServiceHost.Controllers.Dto;

public class TokenResponse
{
    public required string Token { get; init; }

    public required string RefreshToken { get; init; }
}
