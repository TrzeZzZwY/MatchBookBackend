using MatchBook.App.Services.Auth;
using MatchBook.Domain;
using MediatR;

namespace MatchBook.App.Query.RefreshToken;

public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, AuthJwt>
{
    private readonly IAuthService _authService;

    public RefreshTokenQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthJwt> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _authService.RefreshToken(request.AccessToken, request.RefreshToken);
    }
}
