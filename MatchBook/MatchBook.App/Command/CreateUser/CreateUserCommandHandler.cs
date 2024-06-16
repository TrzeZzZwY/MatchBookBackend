using MatchBook.Domain.Exceptions;
using MatchBook.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MatchBook.App.Command.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.NickName,
            RegionId = request.RegionId,
            Email = request.Email
        };

        var result  = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new IdentityException(string.Join(',', result.Errors.Select(e => e.Description)));
        }
    }
}
