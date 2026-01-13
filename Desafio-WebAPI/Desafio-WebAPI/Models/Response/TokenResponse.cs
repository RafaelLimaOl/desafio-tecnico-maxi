namespace Desafio_WebAPI.Models.Response;

public class TokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public Guid UserId { get; set; }
}
