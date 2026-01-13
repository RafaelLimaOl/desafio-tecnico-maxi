using System.Security.Claims;

namespace Desafio_WebAPI.Utils;

public static class UserIdStringToGuid
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException("UserId inválido");

        return userId;
    }
    
}
