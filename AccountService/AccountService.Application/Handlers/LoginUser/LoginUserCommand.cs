using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.LoginUser;
public class LoginUserCommand : IRequest<Result<LoginUserResult, Error>>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}