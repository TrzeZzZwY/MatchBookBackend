using Microsoft.AspNetCore.Identity;

namespace AccountService.Domain.Models;

public class AccountRole : IdentityRole<int>
{
    public AccountRole()
    {
        
    }

    public AccountRole(string role): base(role)
    {
        
    }
}
