namespace AccountService.Application.Handlers.RefreshToken;
public class RefreshTokenResult
{
    public required string Token { get; init; }

    public required string RefreshToken { get; init; }
}