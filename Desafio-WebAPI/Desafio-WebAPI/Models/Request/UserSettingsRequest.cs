namespace Desafio_WebAPI.Models.Request;

public class UserSettingsRequest
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }
}
