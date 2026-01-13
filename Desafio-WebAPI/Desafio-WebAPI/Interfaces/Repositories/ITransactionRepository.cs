using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;

namespace Desafio_WebAPI.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<TransactionResponse>> GetAllByPeopleId(Guid peopleId);
    Task<IEnumerable<TransactionResponse>> GetAllByCategoryId(Guid categoryId);
    Task<IEnumerable<TransactionResponse>> GetAllByUserId(Guid userId);
    Task<TransactionResponse?> GetTransactionById(Guid transactionId);
    Task<TransactionResponse> CreateTransaction(Guid userId, TransactionRequest request);
    Task<bool> UpdateTransaction(Guid userId, Guid transactionId, TransactionRequest request);
    Task<bool> DeleteTransaction(Guid transactionId);
    Task<bool> ExistTransaction(Guid transactionId);
    Task<bool> PeopleHaveTransaction(Guid transactionId, Guid peopleId);
    Task<bool> CategoryHaveTransaction(Guid transactionId, Guid categoryId);
    Task<bool> TransactionContainsInUser(Guid transactionId, Guid userId);

}
