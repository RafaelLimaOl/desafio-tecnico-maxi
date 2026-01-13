namespace Desafio_WebAPI.Models.Response;

public class PeoplePaginationResponse
{
    public List<PeopleResponse>? Peoples { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
