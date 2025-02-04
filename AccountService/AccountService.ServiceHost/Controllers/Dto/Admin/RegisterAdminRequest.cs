using AccountService.Domain.Common;

namespace AccountService.ServiceHost.Controllers.Dto.Admin;

public record RegisterAdminRequest
{
    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Password { get; init; }
}
