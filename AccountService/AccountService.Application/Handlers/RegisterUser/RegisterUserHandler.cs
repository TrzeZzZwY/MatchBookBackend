using AccountService.Domain.Common;
using AccountService.Domain.Models;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult, Error>>
{
    private readonly UserManager<UserAccount> _userManager;

    public RegisterUserHandler(UserManager<UserAccount> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<RegisterUserResult, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not null) return new Error("Cannot create user", ErrorReason.undefined);

        user = new UserAccount
        {
            FistName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.FirstName,
            BirthDate = request.BirthDate
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (createResult.Succeeded) return new RegisterUserResult();

        return new Error(string.Join(',', createResult.Errors.Select(e => e.Description)), ErrorReason.InternalError);
    }
}
