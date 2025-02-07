using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.UserStatus;
public class ChangeUserStatusCommand : IRequest<Result<ChangeUserStatusResult, Error>>
{
    public int UserId { get; init; }

    public AccountStatus Status { get; init; }
}
