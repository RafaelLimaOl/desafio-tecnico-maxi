namespace Desafio_WebAPI.Models.Request;

public class RefreshTokenRequest
{
    public Guid UserId { get; set; }
    public required string RefreshToken { get; set; }
}
