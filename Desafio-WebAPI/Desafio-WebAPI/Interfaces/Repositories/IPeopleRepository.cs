using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;

namespace Desafio_WebAPI.Interfaces.Repositories;

public interface IPeopleRepository
{
    Task<IEnumerable<PeopleResponse>> GetAllPeople(Guid userId);
    Task<PeopleResponse?> GetPeopleById(Guid PeopleId);
    Task<(int totalRecords, List<PeopleResponse> peoples)> GetPeoplePagination(PeopleFilterRequest request);
    Task<PeopleResponse> CreatePeople(Guid userId, PeopleRequest request);
    Task<bool> UpdatePeople(Guid userId, Guid peopleId, PeopleRequest request);
    Task<bool> DeletePeople(Guid peopleId);
    Task<bool> ExistPeople(Guid peopleId);
    Task<bool> PeopleContainsInUser(Guid peopleId, Guid userId);
}
