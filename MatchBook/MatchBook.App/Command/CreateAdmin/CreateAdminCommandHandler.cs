using MatchBook.Domain.Exceptions;
using MatchBook.Domain.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MatchBook.App.Command.CreateUser;

public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand>
{
    private readonly UserManager<ApplicationAdmin> _userManager;

    public CreateAdminCommandHandler(UserManager<ApplicationAdmin> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationAdmin
        {
            UserName = request.Name,
            Email = request.Email
        };

        var result  = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new IdentityException(string.Join(',', result.Errors.Select(e => e.Description)));
        }
    }
}
