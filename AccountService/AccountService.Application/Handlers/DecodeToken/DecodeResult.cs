using AccountService.Domain.Common;

namespace AccountService.Application.Handlers.DecodeToken;
public class DecodeResult
{
    public int UserId { get; init; }

    public List<string> Roles { get; init; }

    public Region Region { get; init; }

    public DateTime ValidFrom { get; init; }

    public DateTime ValidTo { get; init; }
}