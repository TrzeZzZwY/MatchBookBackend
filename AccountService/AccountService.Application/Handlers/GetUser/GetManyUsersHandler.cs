using AccountService.Application.Extensions;
using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AccountService.Application.Handlers.GetUser;

public class GetManyUsersHandler : IRequestHandler<GetManyUsersCommand, Result<PaginatedResult<GetUserResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyUsersHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetUserResult>, Error>> Handle(GetManyUsersCommand request, CancellationToken cancellationToken)
    {
        var users = _databaseContext.UserAccounts.AsQueryable();
        users = users.Include(e => e.Account);

        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            var fullNameRequest = request.FullName.Trim();
            users = users.Where(e => (e.FirstName + " " + e.LastName).Contains(fullNameRequest));
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
            users = users.Where(e => (e.Account.Email.ToLower()).Contains(request.Email.ToLower()));

        if (request.Status is not null)
            users = users.Where(e => e.Account.Status == request.Status);

        var total = users.Count();
        users = users
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return request.PaginationOptions.ToPaginatedResult(
            users.Select(e => new GetUserResult
            {
                Id = e.Id,
                Email = e.Account.Email!,
                FirstName = e.FirstName,
                LastName = e.LastName,
                BirthDate = e.BirthDate,
                Region = e.Region,
                Status = e.Account.Status
            }).ToList(),
            total
            );
    }
}
