using Microsoft.AspNetCore.Identity;

namespace MatchBook.Domain.Models.Identity;

public class ApplicationAdmin : IdentityUser<int>
{
    public int? RefreshTokenId { get; set; }

    public AdminRefreshToken? RefreshToken { get; set; }
}
