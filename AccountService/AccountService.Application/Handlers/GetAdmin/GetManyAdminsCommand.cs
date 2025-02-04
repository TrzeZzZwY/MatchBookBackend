using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.GetAdmin;

public class GetManyAdminsCommand : IRequest<Result<List<GetAdminResult>, Error>>
{
    public required PaginationOptions paginationOptions { get; init; }
}
