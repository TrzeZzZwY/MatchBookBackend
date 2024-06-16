using MatchBook.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MatchBook.WebApi.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = configuration.GetSection("Authentication").Get<AuthOptions>();

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8
                        .GetBytes(authOptions.Secret)
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    SaveSigninToken = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience};
                });
        }
    }
}
