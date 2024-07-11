using MatchBook.Domain;
using MatchBook.Domain.Exceptions;
using MatchBook.Domain.Models.Identity;
using MatchBook.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;
namespace MatchBook.App.Services.Auth;

public class AuthAdminService : IAuthService
{
    private readonly UserManager<ApplicationAdmin> _userManager;
    private readonly AdminDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthAdminService(UserManager<ApplicationAdmin> userManager, AdminDbContext context, ITokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }

    public bool CanHandle(Domain.Enums.ApplicationRole role) => role == Domain.Enums.ApplicationRole.admin;

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
            user.RefreshToken = new Domain.Models.AdminRefreshToken
            {
                Token = refreshToken.Token,
                ExpireDate = refreshToken.ExpireDate
            };
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

        user.RefreshToken = new Domain.Models.AdminRefreshToken
        {
            Token = newRefreshToken.Token,
            ExpireDate = newRefreshToken.ExpireDate
        };
        await _userManager.UpdateAsync(user);

        return new AuthJwt(token: newToken, refreshToken: user.RefreshToken!.Token, userId: user.Id);
    }

    private async Task LoadTokenForUser(ApplicationAdmin user)
    {
        await _context.Users.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
    }
}
