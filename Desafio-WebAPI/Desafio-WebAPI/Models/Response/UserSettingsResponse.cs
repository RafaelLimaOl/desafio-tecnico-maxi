namespace Desafio_WebAPI.Models.Response;

public class UserSettingsResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool? IsActive { get; set; }
}
