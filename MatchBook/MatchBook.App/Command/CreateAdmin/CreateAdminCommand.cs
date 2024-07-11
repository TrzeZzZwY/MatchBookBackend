using MatchBook.Domain.Models;
using MediatR;

namespace MatchBook.App.Command.CreateUser;

public class CreateAdminCommand : IRequest
{
    public required string NickName { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }
}
