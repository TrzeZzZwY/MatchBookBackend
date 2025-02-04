using MediatR;
using CSharpFunctionalExtensions;
using AccountService.Domain.Common;

namespace AccountService.Application.Handlers.RegisterAdmin;

public class RegisterAdminCommand: IRequest<Result<RegisterAdminResult,Error>>
{
    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Password { get; init; }

    public required int CreatedBy { get; set; }
}
