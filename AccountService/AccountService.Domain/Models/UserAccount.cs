using Microsoft.AspNetCore.Identity;

namespace AccountService.Domain.Models;

public class UserAccount : IdentityUser<int>
{
    public required string FistName { get; set; }

    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }
}
