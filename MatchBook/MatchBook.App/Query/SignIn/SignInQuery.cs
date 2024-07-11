using MatchBook.Domain;
using MediatR;

namespace MatchBook.App.Query.SignIn;

public class SignInQuery: IRequest<AuthJwt>
{
    public required string Login { get; set; }

    public required string Password { get; set; }
}
