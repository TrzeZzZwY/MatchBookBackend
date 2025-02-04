using AccountService.Domain.Common;

namespace AccountService.Application.Handlers.GetAdmin;

public record GetAdminResult
{
    public required int Id { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required int? AccountCreatorId { get; set; }
}
