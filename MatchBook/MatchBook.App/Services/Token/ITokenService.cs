using MatchBook.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MatchBook.App.Services;

public interface ITokenService
{
    public string CreateAccessToken(IdentityUser<int> user, IEnumerable<string> roles);
    public RefreshToken CreateRefreshToken();
    public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}
