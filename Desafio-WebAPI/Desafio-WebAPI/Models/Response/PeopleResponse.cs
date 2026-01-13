namespace Desafio_WebAPI.Models.Response;

public class PeopleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public bool IsActive { get; set; }
}
