using AccountService.Domain.Common;

namespace AccountService.ServiceHost.Controllers.Dto.Auth;

public class DecodedToken
{
    public int UserId { get; set; }

    public List<string> Roles { get; set; }

    public Region Region { get; set; }
}