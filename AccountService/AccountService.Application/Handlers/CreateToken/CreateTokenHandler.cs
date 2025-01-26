using AccountService.Application.Services;
using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AccountService.Application.Handlers.CreateToken;
public class CreateTokenHandler : IRequestHandler<CreateTokenCommand, Result<CreateTokenResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Account> _userManager;

    public CreateTokenHandler(DatabaseContext databaseContext, ITokenService tokenService, UserManager<Account> userManager)
    {
        _databaseContext = databaseContext;
        _tokenService = tokenService;
        _userManager = userManager;
    }
    public async Task<Result<CreateTokenResult, Error>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var account = await _databaseContext.Users
            .Where(e => e.UserAccountId == request.AccountId || e.AdminAccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null) return new Error("User not found", ErrorReason.BadRequest);

        if (account.UserAccountId is not null)
            _databaseContext.Entry(account).Reference(e => e.UserAccount).Load();
        if (account.AdminAccountId is not null)
            _databaseContext.Entry(account).Reference(e => e.AdminAccount).Load();

        var roles = await _userManager.GetRolesAsync(account);

        var tokenGenerateResult = await _tokenService.GenerateJsonWebToken(GetClaims(request, roles, account));
        if (tokenGenerateResult.IsFailure) return tokenGenerateResult.Error;

        var refreshToken = await _databaseContext.RefreshTokens.Where(e => e.AccountId == request.AccountId).FirstOrDefaultAsync(cancellationToken);
        if (refreshToken is null || refreshToken.ExpireDate <= DateTime.UtcNow)
        {
            if (refreshToken is null)
            {
                refreshToken = new Domain.Models.RefreshToken
                {
                    AccountId = account.Id,
                };
            }

            refreshToken.ExpireDate = DateTime.UtcNow.AddMinutes(10);
            refreshToken.Token = _tokenService.GenerateRefreshToken();

            _databaseContext.RefreshTokens.Update(refreshToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        return new CreateTokenResult { Token = tokenGenerateResult.Value, RefreshToken = refreshToken.Token };
    }

    private List<Claim> GetClaims(CreateTokenCommand request, IList<string> roles, Account account)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, request.AccountId.ToString()),
        };

        if (roles.Contains("User"))
        {
            claims.Add(new("UserRegion", account.UserAccount!.Region.ToString()));
        }

        foreach (var role in roles)
            claims.Add(new(ClaimTypes.Role, role));

        return claims;
    }
}
