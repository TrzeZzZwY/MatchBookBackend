using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult, Error>>
{
    private readonly UserManager<Account> _userManager;
    private readonly DatabaseContext _context;

    public RegisterUserHandler(UserManager<Account> userManager,
        DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result<RegisterUserResult, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByEmailAsync(request.Email);

        if (account is not null) return new Error("Cannot create user", ErrorReason.undefined);

        account = new Account(request.Email);

        var createResult = await _userManager.CreateAsync(account, request.Password);

        if (!createResult.Succeeded) 
            return new Error(string.Join("\n", createResult.Errors.Select(e => e.Description)), ErrorReason.InternalError);

        var addToRoleResult = await _userManager.AddToRoleAsync(account, "User");
        if (!addToRoleResult.Succeeded)
            return new Error(string.Join("\n", addToRoleResult.Errors.Select(e => e.Description)), ErrorReason.InternalError);

        var user = new UserAccount
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Region = request.Region,
            AccountId = account.Id
        };

        var userEntity = await _context.UserAccounts.AddAsync(user, cancellationToken);
        var a = account.LinkAccountToUser(userEntity.Entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterUserResult();
    }
}
