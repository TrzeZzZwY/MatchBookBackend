using MatchBook.App.Services.Auth;
using MatchBook.Domain;
using MediatR;

namespace MatchBook.App.Query.SignIn;

public class RefreshTokenQueryHandler : IRequestHandler<SignInQuery, AuthJwt>
{
    private readonly IAuthService _authService;

    public RefreshTokenQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthJwt> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        return await _authService.Login(request.Login, request.Password);
    }
}
