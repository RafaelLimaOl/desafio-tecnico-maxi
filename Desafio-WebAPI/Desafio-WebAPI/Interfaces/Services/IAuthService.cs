using Desafio_WebAPI.Models.Entities;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;

namespace Desafio_WebAPI.Interfaces.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterUserRequest request);
    Task<TokenResponse?> LoginAsync(LoginUserRequest request);
    Task<TokenResponse?> RefreshTokenAsync(RefreshTokenRequest request);
}
