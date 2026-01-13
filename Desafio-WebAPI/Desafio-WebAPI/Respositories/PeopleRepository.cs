using Dapper;
using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using System.Data;

namespace Desafio_WebAPI.Respositories;

public class PeopleRepository(IDbConnection dbConnection) : IPeopleRepository
{

    private readonly IDbConnection _dbConnection = dbConnection;

    // Operações do CRUD
    public async Task<IEnumerable<PeopleResponse>> GetAllPeople(Guid userId)
    {
        const string query = @"SELECT Id, Name, Age, IsActive FROM Peoples WHERE UserId = @UserId";
        return [.. (await _dbConnection.QueryAsync<PeopleResponse>(query, new { UserId = userId}))];
    }

    public async Task<PeopleResponse?> GetPeopleById(Guid peopleId)
    {
        const string query = @"SELECT Id, Name, Age, IsActive FROM Peoples WHERE Id = @PeopleId";
        return await _dbConnection.QueryFirstOrDefaultAsync<PeopleResponse>(query, new { PeopleId = peopleId });
    }

    public async Task<(int totalRecords, List<PeopleResponse> peoples)> GetPeoplePagination(PeopleFilterRequest request)
    {

        var sortDirection = request.Order.Equals("DESC", StringComparison.OrdinalIgnoreCase)
            ? "DESC"
            : "ASC";

        var filters = new List<string> { "UserId = @UserId" };
        var sortColumn = request.Sort;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            filters.Add("Name LIKE @SearchTerm");

        var whereClause = "WHERE " + string.Join(" AND ", filters);

        var query = $@"
            SELECT Id, Name, Age, IsActive FROM Peoples {whereClause}
            ORDER BY 
                {sortColumn} {sortDirection}
            OFFSET 
                @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

            SELECT COUNT(*) FROM Peoples {whereClause};
            ";

        var parameters = new
        {
            request.UserId,
            request.Offset,
            request.Limit,
            SearchTerm = $"%{request.SearchTerm}%"
        };

        using var multi = await _dbConnection.QueryMultipleAsync(query, parameters);

        var peoples = (await multi.ReadAsync<PeopleResponse>()).ToList();
        var totalRecords = await multi.ReadFirstAsync<int>();

        return (totalRecords, peoples);
    }

    public async Task<bool> UpdatePeople(Guid userId, Guid peopleId, PeopleRequest request)
    {
        const string query = @"UPDATE Peoples SET Name = @Name, Age = @Age, IsActive = @IsActive WHERE Id = @PeopleId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            request.Name,
            request.Age,
            request.IsActive,
            PeopleId = peopleId
        });

        return rows > 0;
    }

    public async Task<PeopleResponse> CreatePeople(Guid userId, PeopleRequest request)
    {

        const string query = @"INSERT INTO Peoples(Name, Age, UserId) OUTPUT INSERTED.Id VALUES (@Name, @Age, @UserId)";
        var id = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            request.Name,
            request.Age,
            userId
        });

        return new PeopleResponse
        {
            Id = id,
            Name = request.Name,
            Age = request.Age,
            IsActive = true
        };
    }

    public async Task<bool> DeletePeople(Guid peopleId)
    {
        const string query = @"DELETE FROM Peoples WHERE Id = @PeopleId";
        var rows = await _dbConnection.ExecuteAsync(query, new { PeopleId = peopleId });

        return rows > 0;
    }

    // Funções de Validação

    public async Task<bool> ExistPeople(Guid peopleId)
    {
        const string query = @"SELECT 1 FROM Peoples WHERE Id = @PeopleId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { PeopleId = peopleId });
    }

    public async Task<bool> PeopleContainsInUser(Guid peopleId, Guid userId)
    {
        const string query = @"SELECT 1 FROM Peoples WHERE Id = @PeopleId AND UserId = @UserId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { PeopleId = peopleId, UserId = userId });
    }
}
