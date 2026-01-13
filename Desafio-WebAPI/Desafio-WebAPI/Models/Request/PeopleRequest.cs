namespace Desafio_WebAPI.Models.Request;

public class PeopleRequest
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    public bool IsActive { get; set; }
}
