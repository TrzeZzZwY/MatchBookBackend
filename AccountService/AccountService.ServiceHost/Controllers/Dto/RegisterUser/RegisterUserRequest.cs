using AccountService.Domain.Common;

namespace AccountService.ServiceHost.Controllers.Dto.RegisterUser;

public record RegisterUserRequest
{
    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Password { get; init; }

    public required DateTime BithDate { get; init; }

    public required Region Region { get; init; }
}
