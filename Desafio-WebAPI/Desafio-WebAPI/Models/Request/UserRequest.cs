namespace Desafio_WebAPI.Models.Request;

public class RegisterUserRequest
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class LoginUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
