using Dapper;
using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using System.Data;

namespace Desafio_WebAPI.Respositories;

public class TransactionRepository(IDbConnection dbConnection) : ITransactionRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;
    
    public async Task<IEnumerable<TransactionResponse>> GetAllByPeopleId(Guid peopleId)
    {
        const string query = @"SELECT Id, Description, Amount, TransactionType, Status, CategoryId, IsActive FROM Transactions WHERE PeopleId = @PeopleId";
        return [.. (await _dbConnection.QueryAsync<TransactionResponse>(query, new { PeopleId = peopleId }))];
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllByCategoryId(Guid categoryId)
    {
        const string query = @"SELECT Id, Description, Amount, TransactionType, Status, PeopleId, IsActive FROM Transactions WHERE CategoryId = @CategoryId";
        return [.. (await _dbConnection.QueryAsync<TransactionResponse>(query, new { CategoryId = categoryId }))];
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllByUserId(Guid userId)
    {
        const string query = @"SELECT Id, Description, Amount, TransactionType, Status, PeopleId, CategoryId, UserId, IsActive FROM Transactions WHERE UserId = @UserId";
        return [.. (await _dbConnection.QueryAsync<TransactionResponse>(query, new { UserId = userId }))];
    }

    public async Task<TransactionResponse?> GetTransactionById(Guid transactionId)
    {
        const string query = @"SELECT Id, Description, Amount, PeopleId, CategoryId, TransactionType, Status, IsActive FROM Transactions WHERE Id = @TransactionId";
        return await _dbConnection.QueryFirstOrDefaultAsync<TransactionResponse>(query, new { TransactionId = transactionId });
    }

    public async Task<TransactionResponse> CreateTransaction(Guid userId, TransactionRequest request)
    {
        const string query = @"INSERT INTO Transactions(Description, Amount, TransactionType, Status, PeopleId, CategoryId, UserId) OUTPUT INSERTED.Id VALUES (@Description, @Amount, @TransactionType, @Status, @PeopleId, @CategoryId, @UserId)";
        var id = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            request.Description,
            request.Amount,
            request.TransactionType,
            request.Status,
            request.PeopleId,
            request.CategoryId,
            userId
        });

        return new TransactionResponse
        {
            Id = id,
            Description = request.Description,
            Amount = request.Amount,
            Status = request.Status,
            PeopleId = request.PeopleId,
            UserId = userId,
            CategoryId = request.CategoryId,
            IsActive = true
        };
    }

    public async Task<bool> UpdateTransaction(Guid userId, Guid transactionId, TransactionRequest request)
    {
        const string query = @"UPDATE Transactions SET Description = @Description, Amount = @Amount, TransactionType = @TransactionType, Status = @Status, PeopleId = @PeopleId, CategoryId = @CategoryId, IsActive = @IsActive WHERE Id = @TransactionId";
        var rows = await _dbConnection.ExecuteAsync(query, new 
        { 
            request.Description,
            request.Amount,
            request.TransactionType,
            request.Status,
            request.PeopleId,
            request.CategoryId,
            request.IsActive,
            TransactionId = transactionId 
        });

        return rows > 0;
    }
    public async Task<bool> DeleteTransaction(Guid transactionId)
    {
        const string query = @"DELETE FROM Transactions WHERE Id = @TransactionId";
        var rows = await _dbConnection.ExecuteAsync(query, new { TransactionId = transactionId });

        return rows > 0;
    }

    // Funções de validação

    public async Task<bool> ExistTransaction(Guid transactionId)
    {
        const string query = @"SELECT 1 FROM Transactions WHERE Id = @TransactionId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { TransactionId = transactionId });
    }

    public async Task<bool> PeopleHaveTransaction(Guid transactionId, Guid peopleId)
    {
        const string query = @"SELECT 1 FROM Peoples WHERE Id = @PeopleId AND TransactionId = @TransactionId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { TransactionId = transactionId, PeopleId = peopleId });
    }
    public async Task<bool> CategoryHaveTransaction(Guid transactionId, Guid categoryId)
    {
        const string query = @"SELECT 1 FROM Categories WHERE Id = @CategoryId AND TransactionId = @transactionId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { TransactionId = transactionId, CategoryId = categoryId });
    }
   
    public async Task<bool> TransactionContainsInUser(Guid transactionId, Guid userId)
    {
        const string query = @"SELECT 1 FROM Transactions WHERE Id = @TransactionId AND UserId = @UserId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { TransactionId = transactionId, UserId = userId });
    }

}
