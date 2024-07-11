using MatchBook.Domain;
using MatchBook.Domain.Enums;

namespace MatchBook.App.Services.Auth;

public interface IAuthService
{
    Task<AuthJwt> Login(string email, string password);
    Task<AuthJwt> RefreshToken(string token, string refreshToken);
    bool CanHandle(ApplicationRole role);
}

