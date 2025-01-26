using AccountService.Application.Services;
using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AccountService.Application.Handlers.RefreshToken;
public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly ITokenService _tokenService;

    public RefreshTokenHandler(DatabaseContext databaseContext, ITokenService tokenService)
    {
        _databaseContext = databaseContext;
        _tokenService = tokenService;
    }

    public async Task<Result<RefreshTokenResult, Error>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await _tokenService.ValidateToken(request.ExpiredToken, ignoreExpiredDate: true);
        if (validateResult.IsFailure) return validateResult.Error;

        var readTokenResult = await _tokenService.ReadToken(request.ExpiredToken);
        if (readTokenResult.IsFailure) return readTokenResult.Error;

        var claims = readTokenResult.Value.Claims;
   
        var userId = int.Parse(claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier).Value);
        
        var generateTokenResult = await _tokenService.GenerateJsonWebToken(claims);
        if (generateTokenResult.IsFailure) return generateTokenResult.Error;

        var refreshToken = await _databaseContext.RefreshTokens
            .Where(e => e.Token == request.RefreshToken && e.AccountId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (refreshToken is null || refreshToken.ExpireDate < DateTime.UtcNow)
            return new Error("Expired or incorrect refresh token", ErrorReason.Unauthorized);

        refreshToken.Token = _tokenService.GenerateRefreshToken();
        refreshToken.ExpireDate = DateTime.UtcNow.AddMinutes(30);
        _databaseContext.RefreshTokens.Update(refreshToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResult { Token = generateTokenResult.Value, RefreshToken = refreshToken.Token };
    }
}