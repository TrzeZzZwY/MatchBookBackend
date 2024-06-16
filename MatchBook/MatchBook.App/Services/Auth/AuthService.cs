using MatchBook.Domain;
using MatchBook.Domain.Exceptions;
using MatchBook.Domain.Models;
using MatchBook.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
namespace MatchBook.App.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, AppDbContext context, ITokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthJwt> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new LoginCredentialsException();
        await LoadTokenForUser(user);

        if (!await _userManager.CheckPasswordAsync(user, password)) throw new LoginCredentialsException();

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.CreateAccessToken(user, roles);

        if (user.RefreshToken == null || user.RefreshToken?.ExpireDate < DateTime.UtcNow)
        {
            var refreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
        }

        return new AuthJwt(token: token, refreshToken: user.RefreshToken!.Token, userId: user.Id);
    }

    public async Task<AuthJwt> RefreshToken(string token, string refreshToken)
    {
        var pricipal = await _tokenService.GetPrincipalFromExpiredToken(token);
        var user = await _userManager.GetUserAsync(pricipal) ?? throw new UserNotFoundException();
        await LoadTokenForUser(user);

        if (user.RefreshToken?.Token != refreshToken) throw new TokenValidationException();

        var roles = await _userManager.GetRolesAsync(user);
        var newToken = _tokenService.CreateAccessToken(user, roles);
        var newRefreshToken = _tokenService.CreateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new AuthJwt(token: newToken, refreshToken: user.RefreshToken!.Token, userId: user.Id);
    }

    private async Task LoadTokenForUser(ApplicationUser user)
    {
        await _context.Users.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
    }
}
