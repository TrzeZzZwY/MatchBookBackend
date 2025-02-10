using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.GetUser;

public class GetUserHandler : IRequestHandler<GetUserCommand, Result<GetUserResult?, Error>>
{
    private readonly UserManager<Account> _userManager;
    private readonly DatabaseContext _context;

    public GetUserHandler(UserManager<Account> userManager,
        DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result<GetUserResult?, Error>> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserAccounts.FindAsync([request.Id], cancellationToken);

        await _context.Entry(user).Reference(e => e.Account).LoadAsync();

        return user is null ?
            null :
            new GetUserResult
            {
                Id = user.Id,
                Email = user.Account.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Region = user.Region,
                Status = user.Account.Status
            };
    }
}
