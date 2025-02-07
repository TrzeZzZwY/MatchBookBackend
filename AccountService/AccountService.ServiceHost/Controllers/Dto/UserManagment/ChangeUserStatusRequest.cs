using AccountService.Domain.Common;

namespace AccountService.ServiceHost.Controllers.Dto.UserManagment;

public class ChangeUserStatusRequest
{
    public int UserId { get; init; }

    public AccountStatus Status { get; init; }
}
