using AccountService.Application.Extensions;
using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Handlers.GetAdmin;

public class GetManyAdminsHandler : IRequestHandler<GetManyAdminsCommand, Result<PaginatedResult<GetAdminResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyAdminsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetAdminResult>, Error>> Handle(GetManyAdminsCommand request, CancellationToken cancellationToken)
    {
        //Todo: apply filters in future
        var users = _databaseContext.AdminAccounts.AsQueryable();

        var total = users.Count();

        users = users
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return request.PaginationOptions.ToPaginatedResult(
            users.Select(e => new GetAdminResult
            {
                Id = e.Id,
                Email = e.Account.Email!,
                FirstName = e.FirstName,
                LastName = e.LastName,
                AccountCreatorId = e.AccountCreatorId
            }).ToList(),
            total
            );
    }
}
