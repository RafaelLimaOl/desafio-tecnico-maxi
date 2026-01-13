using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;

namespace Desafio_WebAPI.Interfaces.Services;

public interface ITransactionService
{
    Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByPeople(Guid peopleId);
    Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByCategory(Guid categoryId);
    Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByUser(Guid userId);
    Task<ServiceResponse<TransactionResponse>> GetTransactionById(Guid transactionId);
    Task<ServiceResponse<TransactionResponse>> CreateTransaction(Guid userId, TransactionRequest request);
    Task<ServiceResponse<TransactionResponse>> UpdateTransaction(Guid userId, Guid transactionId, TransactionRequest request);
    Task<ServiceResponse<bool>> DeleteTransaction(Guid userId, Guid peopleId);
}
