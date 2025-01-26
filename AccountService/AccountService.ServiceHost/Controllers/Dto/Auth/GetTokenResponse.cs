namespace AccountService.ServiceHost.Controllers.Dto.Auth;

public class GetTokenResponse
{
    public string Token { get; init; }

    public string RefreshToken { get; init; }
}