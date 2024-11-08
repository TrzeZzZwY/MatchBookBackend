using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.GetUser;

public class GetManyUsersCommand : IRequest<Result<List<GetUserResult>, Error>>
{
    public required PaginationOptions paginationOptions { get; init; }
}
