using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccountService.Application.Services;
public interface ITokenService
{
    Task<Result<string,Error>> GenerateJsonWebToken(IEnumerable<Claim> claims);

    Task<Result<JwtSecurityToken,Error>> ReadToken(string token);

    Task<Result<TokenValidationResult, Error>> ValidateToken(string token, bool ignoreExpiredDate = false);

    string GenerateRefreshToken();
}
