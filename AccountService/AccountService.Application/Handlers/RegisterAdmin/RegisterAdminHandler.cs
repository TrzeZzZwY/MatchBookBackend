using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.RegisterAdmin;

public class RegisterAdminHandler : IRequestHandler<RegisterAdminCommand, Result<RegisterAdminResult, Error>>
{
    private readonly UserManager<Account> _userManager;
    private readonly DatabaseContext _context;

    public RegisterAdminHandler(UserManager<Account> userManager,
        DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result<RegisterAdminResult, Error>> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByEmailAsync(request.Email);
        var creatorAccount = await _userManager.FindByIdAsync(request.CreatedBy.ToString());

        if (account is not null ||
            creatorAccount is null ||
            creatorAccount.AdminAccountId is null) return new Error("Cannot create admin", ErrorReason.undefined);

        account = new Account(request.Email);

        var createResult = await _userManager.CreateAsync(account, request.Password);

        if (!createResult.Succeeded) 
            return new Error(string.Join("\n", createResult.Errors.Select(e => e.Description)), ErrorReason.InternalError);

        var addToRoleResult = await _userManager.AddToRoleAsync(account, "Admin");
        if (!addToRoleResult.Succeeded)
            return new Error(string.Join("\n", addToRoleResult.Errors.Select(e => e.Description)), ErrorReason.InternalError);

        var admin = new AdminAccount
        {
            FistName = request.FirstName,
            LastName = request.LastName,
            Account = account,
            AccountCreatorId = creatorAccount.AdminAccountId
        };

        var userEntity = await _context.AdminAccounts.AddAsync(admin, cancellationToken);
        var a = account.LinkAccountToAdmin(userEntity.Entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterAdminResult();
    }
}
