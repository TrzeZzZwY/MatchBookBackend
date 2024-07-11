using MatchBook.App.Services.Auth;
using MatchBook.Domain;
using MediatR;

namespace MatchBook.App.Query.SignIn;

public class RefreshTokenQueryHandler : IRequestHandler<SignInQuery, AuthJwt>
{
    private readonly IEnumerable<IAuthService> _authServices;

    public RefreshTokenQueryHandler(IEnumerable<IAuthService> authServices)
    {
        _authServices = authServices;
    }

    public async Task<AuthJwt> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        return await _authServices
            .FirstOrDefault(e => e.CanHandle(request.Role))
            .Login(request.Login, request.Password);
    }
}
