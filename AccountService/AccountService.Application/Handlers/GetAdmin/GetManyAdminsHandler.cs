using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Handlers.GetAdmin;

public class GetManyAdminsHandler : IRequestHandler<GetManyAdminsCommand, Result<List<GetAdminResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyAdminsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<GetAdminResult>, Error>> Handle(GetManyAdminsCommand request, CancellationToken cancellationToken)
    {
        //Todo: apply filters in future
        var users = await _databaseContext.AdminAccounts
            .Skip((request.paginationOptions.PageNumber - 1) * request.paginationOptions.PageSize)
            .Take(request.paginationOptions.PageSize)
            .Select(e => new GetAdminResult { 
                Id = e.Id,
                Email = e.Account.Email!,
                FirstName = e.FistName,
                LastName = e.LastName,
                AccountCreatorId = e.AccountCreatorId})
            .ToListAsync(cancellationToken);

        return users;
    }
}
