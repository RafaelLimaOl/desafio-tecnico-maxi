using Dapper;
using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using System.Data;

namespace Desafio_WebAPI.Respositories;

public class CategoryRepository(IDbConnection dbConnection) : ICategoryRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    // Operações do CRUD
    public async Task<IEnumerable<CategoryResponse>> GetAllCategories(Guid userId)
    {
        const string query = @"SELECT Id, Description, CategoryType, IsActive FROM Categories WHERE UserId = @UserId";
        return [.. (await _dbConnection.QueryAsync<CategoryResponse>(query, new { UserId = userId }))];
    }

    public async Task<CategoryResponse?> GetCategoryById(Guid categoryId)
    {
        const string query = @"SELECT Id, Description, CategoryType, IsActive FROM Categories WHERE Id = @CategoryId";
        return await _dbConnection.QueryFirstOrDefaultAsync<CategoryResponse>(query, new { CategoryId = categoryId });
    }

    public async Task<bool> UpdateCategory(Guid userId, Guid categoryId, CategoryRequest request)
    {
        const string query = @"UPDATE Categories SET Description = @Description, CategoryType = @CategoryType, IsActive = @IsActive WHERE Id = @CategoryId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            request.Description,
            request.CategoryType,
            request.IsActive,
            CategoryId = categoryId
        });

        return rows > 0;
    }

    public async Task<CategoryResponse> CreateCategory(Guid userId, CategoryRequest request)
    {

        const string query = @"INSERT INTO Categories(Description, CategoryType, UserId) OUTPUT INSERTED.Id VALUES (@Description, @CategoryType, @UserId)";
        var id = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            request.Description,
            request.CategoryType,
            userId
        });

        return new CategoryResponse
        {
            Id = id,
            Description = request.Description,
            CategoryType = request.CategoryType,
            IsActive = true
        };
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        const string query = @"DELETE FROM Transactions WHERE CategoryId = @CategoryId; DELETE FROM Categories WHERE Id = @CategoryId;";
        var rows = await _dbConnection.ExecuteAsync(query, new { CategoryId = categoryId });

        return rows > 0;
    }

    // Funções de Validação

    public async Task<bool> ExistCategory(Guid categoryId)
    {
        const string query = @"SELECT 1 FROM Categories WHERE Id = @CategoryId";
        var categoryExist = await _dbConnection.ExecuteScalarAsync<int>(query, new { CategoryId = categoryId });

        if (categoryExist == 0) return false;

        return true;
    }

    public async Task<bool> CategoryContainsInUser(Guid categoryId, Guid userId)
    {
        const string sql = @"SELECT 1 FROM Categories WHERE Id = @CategoryId AND UserId = @UserId";

        return await _dbConnection.ExecuteScalarAsync<bool>(sql, new { CategoryId = categoryId, UserId = userId });
    }
}
