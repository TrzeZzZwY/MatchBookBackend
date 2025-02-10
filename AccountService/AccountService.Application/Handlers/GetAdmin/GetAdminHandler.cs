using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.GetAdmin;

public class GetAdminHandler : IRequestHandler<GetAdminCommand, Result<GetAdminResult?, Error>>
{
    private readonly UserManager<Account> _userManager;
    private readonly DatabaseContext _context;

    public GetAdminHandler(UserManager<Account> userManager,
        DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result<GetAdminResult?, Error>> Handle(GetAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await _context.AdminAccounts.FindAsync([request.Id], cancellationToken);

        return admin is null ?
            null :
            new GetAdminResult
            {
                Id = admin.Id,
                Email = admin.Account.Email!,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                AccountCreatorId = admin.AccountCreatorId
            };
    }
}
