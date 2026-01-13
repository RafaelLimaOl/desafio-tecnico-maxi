using Dapper;
using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using System.Data;

namespace Desafio_WebAPI.Respositories;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<UserSettingsResponse?> GetUserSettings(Guid userId)
    {
        const string query = @"SELECT Id, Username, Email, IsActive FROM Users WHERE Id = @UserId";
        return await _dbConnection.QueryFirstOrDefaultAsync<UserSettingsResponse?>(query, new { UserId = userId });
    }

    public async Task<bool> EditUserSettings(Guid userId, UserSettingsRequest request)
    {
        const string query = @"UPDATE Users SET Username = @Username, Email = @Email, IsActive = @IsActive WHERE Id = @UserId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            request.UserName,
            request.Email,
            request.IsActive,
            UserId = userId
        });

        return rows > 0;
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        const string query = @"DELETE FROM Users WHERE Id = @UserId";
        var rows = await _dbConnection.ExecuteAsync(query, new { UserId = userId });

        return rows > 0;
    }

    public async Task<bool> ExistUser(Guid userId)
    {
        const string query = @"SELECT COUNT(1) FROM Users WHERE Id = @UserId";
        var userExist = await _dbConnection.ExecuteScalarAsync<int>(query, new { UserId = userId });

        if (userExist == 0) return false;

        return true;
    }


}
