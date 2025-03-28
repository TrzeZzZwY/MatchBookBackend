﻿
using System.Security.Claims;

namespace ReportingService.ServiceHost.Extenions;

public static class UserExtensions
{
    public static int? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        var value = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (value is null) return null;
        return int.Parse(value);
    }

    public static List<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        var values = claimsPrincipal.FindAll(ClaimTypes.Role).Select(e => e.Value);
        if (values is null || !values.Any()) return null;
        return values.ToList();
    }

    public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
    {
        var values = claimsPrincipal.GetRoles();
        return values.Contains("Admin");
    }
    public static bool IsUser(this ClaimsPrincipal claimsPrincipal)
    {
        var values = claimsPrincipal.GetRoles();
        return values.Contains("User");
    }
}