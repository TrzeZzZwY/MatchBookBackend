using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace MatchBook.Domain.Models;

public class ApplicationAdminRole : IdentityRole<int>
{
}
