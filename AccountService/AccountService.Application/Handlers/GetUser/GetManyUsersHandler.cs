using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Handlers.GetUser;

public class GetManyUsersHandler : IRequestHandler<GetManyUsersCommand, Result<List<GetUserResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyUsersHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<GetUserResult>, Error>> Handle(GetManyUsersCommand request, CancellationToken cancellationToken)
    {
        //Todo: apply filters in future
        var users = await _databaseContext.UserAccounts
            .Skip((request.paginationOptions.PageNumber - 1) * request.paginationOptions.PageSize)
            .Take(request.paginationOptions.PageSize)
            .Select(e => new GetUserResult { 
                Id = e.Id,
                Email = e.Account.Email!,
                FirstName = e.FistName,
                LastName = e.LastName,
                BirthDate = e.BirthDate })
            .ToListAsync(cancellationToken);

        return users;
    }
}
