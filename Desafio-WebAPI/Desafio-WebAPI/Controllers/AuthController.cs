using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Entities;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterAsync(RegisterUserRequest request)
    {
        var user = await authService.RegisterAsync(request);
        if (user is null)
            throw new ProblemExeption("Email ou usuário são inválidos", $"{user}", StatusCodes.Status400BadRequest);

        return Ok(new ApiResponse<User>(true, "Conta cadastrada com sucesso", user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login(LoginUserRequest request)
    {
        var result = await authService.LoginAsync(request);

        if (result is null)
            throw new ProblemExeption("Nome ou senha incorretos", $"{result}", StatusCodes.Status400BadRequest);

        return Ok(new ApiResponse<TokenResponse>(true, "Login realizado com sucesso", result));
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var result = await authService.RefreshTokenAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
            throw new ProblemExeption("Refresh token é inválido", $"{result}", StatusCodes.Status400BadRequest);

        return Ok(new ApiResponse<TokenResponse>(true, "Refresh Token  sucesso", result));
    }
}
