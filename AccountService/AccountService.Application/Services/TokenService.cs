using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AccountService.Application.Services;
public class TokenService : ITokenService
{
    private readonly JwtBearerOptions _jwtOptions;

    public TokenService(IOptionsMonitor<JwtBearerOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme);
    }

    public async Task<Result<string, Error>> GenerateJsonWebToken(IEnumerable<Claim> claims)
    {
        try
        {
            var signinCredentials = new SigningCredentials(_jwtOptions.TokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: signinCredentials,
                    issuer : "https://localhost:8900"
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return token;
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }

    }

    public async Task<Result<JwtSecurityToken, Error>> ReadToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            return jwt;
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }

    public async Task<Result<TokenValidationResult, Error>> ValidateToken(string token, bool ignoreExpiredDate = false)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = _jwtOptions.TokenValidationParameters;
            tokenValidationParameters.ValidateLifetime = !ignoreExpiredDate;

            var validateResult = await handler.ValidateTokenAsync(token, tokenValidationParameters);
            if (!validateResult.IsValid)
                return new Error("Invalid token", ErrorReason.Unauthorized);

            return validateResult;
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
