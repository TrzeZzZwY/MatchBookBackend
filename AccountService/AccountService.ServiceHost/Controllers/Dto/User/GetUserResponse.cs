using AccountService.Domain.Common;

namespace AccountService.ServiceHost.Controllers.Dto.RegisterUser;

public class GetUserResponse
{
    public required int Id { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }

    public required Region Region { get; set; }

    public required AccountStatus Status { get; set; }
}
