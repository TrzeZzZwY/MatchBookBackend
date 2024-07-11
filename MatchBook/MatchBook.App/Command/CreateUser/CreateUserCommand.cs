using MatchBook.Domain.Models;
using MediatR;

namespace MatchBook.App.Command.CreateUser;

public class CreateUserCommand : IRequest
{
    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string Name { get; init; }

    public required string SureName { get; init; }

    public required string NickName { get; init; }

    public required int RegionId { get; init; }
}
