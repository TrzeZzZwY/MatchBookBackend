using MatchBook.Domain.Models;
using System.Security.Claims;

namespace MatchBook.App.Services;

public interface ITokenService
{
    public string CreateAccessToken(ApplicationUser user, IEnumerable<string> roles);
    public RefreshToken CreateRefreshToken();
    public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}
