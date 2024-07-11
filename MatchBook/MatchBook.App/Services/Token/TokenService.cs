using MatchBook.Domain.Exceptions;
using MatchBook.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;


namespace MatchBook.App.Services.Token;

public class TokenService : ITokenService
{
    private const int TokenLifetime = 120;
    private const string TokenAlgorithm = SecurityAlgorithms.HmacSha256;
    private readonly JwtBearerOptions _jwtOptions;

    public TokenService(IOptionsMonitor<JwtBearerOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme);
    }
    public string CreateAccessToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var descriptor = new SecurityTokenDescriptor();
        var handler = new JsonWebTokenHandler();

        handler.SetDefaultTimesOnTokenCreation = false;

        var tokenClaims = new Dictionary<string, object>
        {
            { ClaimTypes.NameIdentifier, user.Id },
            { JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(TokenLifetime).ToUnixTimeMilliseconds() },
            { ClaimTypes.Role, roles }
        };

        descriptor.Claims = tokenClaims;
        descriptor.NotBefore = DateTime.UtcNow;
        descriptor.Expires = DateTime.UtcNow.AddMinutes(TokenLifetime);
        descriptor.SigningCredentials = new SigningCredentials(_jwtOptions.TokenValidationParameters.IssuerSigningKey, TokenAlgorithm);

        string token = handler.CreateToken(descriptor);
        return token;
    }

    public RefreshToken CreateRefreshToken()
    {
        RefreshToken token = new RefreshToken()
        {
            Token = GenerateRefreshToken(),
            ExpireDate = DateTime.Now.AddDays(7)
        };

        return token;
    }

    public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = _jwtOptions.TokenValidationParameters;
        tokenValidationParameters.ValidateLifetime = false;

        var handler = new JsonWebTokenHandler();

        var validateResult = await handler.ValidateTokenAsync(token, tokenValidationParameters);

        if (!validateResult.IsValid) throw new TokenValidationException();

        return new ClaimsPrincipal(validateResult.ClaimsIdentity);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
