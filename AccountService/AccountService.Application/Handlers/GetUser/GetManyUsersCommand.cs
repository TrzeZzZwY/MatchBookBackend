using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.GetUser;

public class GetManyUsersCommand : IRequest<Result<PaginatedResult<GetUserResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public string? FullName { get; init; } = null;

    public string? Email { get; init; } = null;

    public AccountStatus? Status { get; init; } = null;
}
