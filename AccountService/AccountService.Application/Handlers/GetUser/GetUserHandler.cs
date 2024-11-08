using AccountService.Domain.Common;
using AccountService.Domain.Models;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Handlers.GetUser;

public class GetUserHandler : IRequestHandler<GetUserCommand, Result<GetUserResult?, Error>>
{
    private readonly UserManager<UserAccount> _userManager;

    public GetUserHandler(UserManager<UserAccount> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<GetUserResult?, Error>> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        return user is null ?
            null :
            new GetUserResult
            {
                Email = user.Email,
                FirstName = user.FistName,
                LastName = user.LastName
            };
    }
}
