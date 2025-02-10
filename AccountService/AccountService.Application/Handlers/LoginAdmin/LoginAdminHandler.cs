using AccountService.Domain.Common;
using AccountService.Domain.Models;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.LoginAdmin;
public class LoginAdminHandler : IRequestHandler<LoginAdminCommand, Result<LoginAdminResult, Error>>
{
    private readonly UserManager<Account> _userManager;

    public LoginAdminHandler(UserManager<Account> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<LoginAdminResult, Error>> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByEmailAsync(request.Email);
        if (account is null || !await _userManager.CheckPasswordAsync(account, request.Password) || account.AdminAccountId is null)
            return new Error("Invalid email or password", ErrorReason.BadRequest);

        if (account.Status == AccountStatus.REMOVED || account.Status == AccountStatus.BANED)
            return new Error("Cannot login to this account", ErrorReason.BadRequest);

        return new LoginAdminResult
        {
            AccountId = account.Id,
            AdminId = (int)account.AdminAccountId
        };
    }
}
