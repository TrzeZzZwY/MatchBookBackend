using MediatR;
using CSharpFunctionalExtensions;
using AccountService.Domain.Common;

namespace AccountService.Application.Handlers.RegisterUser;

public class RegisterUserCommand: IRequest<Result<RegisterUserResult,Error>>
{
    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Password { get; init; }
}
