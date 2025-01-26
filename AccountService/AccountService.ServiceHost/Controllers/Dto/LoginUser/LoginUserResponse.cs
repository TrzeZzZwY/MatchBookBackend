namespace AccountService.ServiceHost.Controllers.Dto.LoginUser;

public class LoginUserResponse
{
    public required string Token { get; init; }

    public required string RefreshToken { get; init; }
}
