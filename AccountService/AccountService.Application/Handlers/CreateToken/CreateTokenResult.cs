namespace AccountService.Application.Handlers.CreateToken;
public class CreateTokenResult
{
    public required string Token { get; init; }

    public required string RefreshToken { get; init; }
}
