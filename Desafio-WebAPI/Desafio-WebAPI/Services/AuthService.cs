using Desafio_WebAPI.Data;
using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Entities;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Desafio_WebAPI.Services;

public class AuthService(DataContext context, IConfiguration configuration) : IAuthService
{
    public async Task<TokenResponse?> LoginAsync(LoginUserRequest request)
    {
        // Será selecionado um usuário que possua o mesmo Email fornecido
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // Caso não exista retorne nulo
        if (user is null)
            return null;

        // Validação da senha 
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return null;

        // Caso exista o usuário e a senha esteja correta Retorne o Token
        TokenResponse response = await CreateTokenResponse(user);

        return response;
    }

    public async Task<TokenResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Aguardo da validação do Token fornecido
        var user = await ValidateRefreshTokenAsync(request.RefreshToken);

        // Caso não exista
        if (user is null)
            return null;

        // Retorno do novo token:
        return await CreateTokenResponse(user);
    }

    public async Task<User?> RegisterAsync(RegisterUserRequest request)
    {
        // Validação dos parâmetros passados:
        if (request.Email is null || await context.Users.AnyAsync(u => u.Email == request.Email))
            return null;

        // Registro do novo usuário
        var user = new User();

        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
        user.Username = request.Username;
        user.PasswordHash = hashedPassword;
        user.Email = request.Email;

        // Salvar as alterações no banco de dados:
        context.Users.Add(user);
        await context.SaveChangesAsync();
    
        return user;
    }

    // Métodos Privados

    private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
    {
        // Validação do RefreshToken
        // Verificação se o tempo do token está válido e não expirou
        // Verificação se o token passado condiz com o token no banco
        return await context.Users.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken &&
            u.RefreshTokenExpiryTime > DateTime.UtcNow);
    }

    private static string GenerateRefreshToken()
    {
        // Geração das parâmetros para o Refresh Token
        var randomNumber = new Byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        // Valor retornado
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateSaveRefreshTokenAsync(User user)
    {
        // Gerar o refrshToken
        var refreshToken = GenerateRefreshToken();
        
        // Atribuição do Token
        user.RefreshToken = refreshToken;
        // Tempo de expiração
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        // Save as alterações no banco de dados e retorne o RefreshToken
        await context.SaveChangesAsync();
        return refreshToken;
    }

    private async Task<TokenResponse> CreateTokenResponse(User? user)
    {
        // Resposta da API com os tokens:
        return new TokenResponse
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateSaveRefreshTokenAsync(user),
            UserId = user.Id
        };
    }

    private string CreateToken(User user)
    {
        // Criação do token:
        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

        // Configuração do JWT
        var newKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
        var credentials = new SigningCredentials(newKey, SecurityAlgorithms.HmacSha512);

        // Token retornado com os seguintes parâmetros:
        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
