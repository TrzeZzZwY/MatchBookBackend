using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.LoginAdmin;
public class LoginAdminCommand : IRequest<Result<LoginAdminResult, Error>>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}