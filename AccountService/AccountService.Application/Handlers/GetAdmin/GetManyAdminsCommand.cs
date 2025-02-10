using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.GetAdmin;

public class GetManyAdminsCommand : IRequest<Result<PaginatedResult<GetAdminResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }
}
