using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;

namespace Desafio_WebAPI.Interfaces.Services;

public interface IPeopleService
{
    Task<ServiceResponse<IEnumerable<PeopleResponse>>> GetAll(Guid userId);
    Task<ServiceResponse<PeopleResponse>> GetPeopleById(Guid peopleId);
    // Selecionar todas as transações por pessoa
    Task<ServiceResponse<PeoplePaginationResponse>> GetPeoplePagination(PeopleFilterRequest request);
    Task<ServiceResponse<PeopleResponse>> CreatePeople(Guid userId, PeopleRequest request);
    Task<ServiceResponse<PeopleResponse>> UpdatePeople(Guid userId, Guid peopleId, PeopleRequest request);
    Task<ServiceResponse<bool>> DeletePeople(Guid userId, Guid peopleId);
}
