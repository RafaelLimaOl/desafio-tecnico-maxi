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
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null)
            return null;

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return null;

        TokenResponse response = await CreateTokenResponse(user);

        return response;
    }

    public async Task<TokenResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await ValidateRefreshTokenAsync(request.RefreshToken);
        if (user is null)
            return null;

        return await CreateTokenResponse(user);
    }

    public async Task<User?> RegisterAsync(RegisterUserRequest request)
    {
        if (request.Email is null || await context.Users.AnyAsync(u => u.Email == request.Email))
            return null;

        var user = new User();

        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
        user.Username = request.Username;
        user.PasswordHash = hashedPassword;
        user.Email = request.Email;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    // Métodos Privados

    private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
    {
        return await context.Users.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken &&
            u.RefreshTokenExpiryTime > DateTime.UtcNow);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new Byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await context.SaveChangesAsync();
        return refreshToken;
    }

    private async Task<TokenResponse> CreateTokenResponse(User? user)
    {
        return new TokenResponse
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateSaveRefreshTokenAsync(user),
            UserId = user.Id
        };
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

        var newKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
        var credentials = new SigningCredentials(newKey, SecurityAlgorithms.HmacSha512);

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
