using System.Security.Claims;

namespace Desafio_WebAPI.Utils;

public static class UserIdStringToGuid
{
    // Função para retornar o UserId do Token fornecido
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validação se o userId é do Tipo Guid
        if (!Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException("UserId inválido");

        return userId;
    }
    
}
