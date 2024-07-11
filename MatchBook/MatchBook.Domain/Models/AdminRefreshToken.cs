using MatchBook.Domain.Models.Identity;

namespace MatchBook.Domain.Models;

public class AdminRefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; }

    public DateTime ExpireDate { get; set; }

    public int UserId { get; set; }

    public ApplicationAdmin User { get; set; }
}
