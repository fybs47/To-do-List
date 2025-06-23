using System.Security.Claims;
using Application.Exceptions;

namespace Application.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        throw new UnauthorizedException("Invalid user ID in token");
    }

    public static string GetUserRole(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Role)?.Value 
               ?? throw new UnauthorizedException("Role claim missing");
    }
}