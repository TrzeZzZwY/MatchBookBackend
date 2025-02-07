using AccountService.Domain.Common;
using AccountService.Domain.Models;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.LoginUser;
public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResult, Error>>
{
    private readonly UserManager<Account> _userManager;

    public LoginUserHandler(UserManager<Account> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<LoginUserResult, Error>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByEmailAsync(request.Email);
        if (account is null || !await _userManager.CheckPasswordAsync(account, request.Password) || account.UserAccountId is null)
            return new Error("Invalid email or password", ErrorReason.BadRequest);

        if (account.Status == AccountStatus.REMOVED || account.Status == AccountStatus.BANED)
            return new Error("Cannot login to this account", ErrorReason.BadRequest);

        return new LoginUserResult
        {
            AccountId = account.Id,
            UserId = (int)account.UserAccountId
        };
    }
}
