using MatchBook.Domain;
using MediatR;

namespace MatchBook.App.Query.RefreshToken;

public class RefreshTokenQuery: IRequest<AuthJwt>
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}
