namespace Desafio_WebAPI.Models.Request;

public class PeopleFilterRequest
{
    public Guid UserId { get; set; }

    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 10;
    public string Sort { get; set; } = string.Empty;
    public string Order { get; set; } = "ASC";
    public string? SearchTerm { get; set; }
    public DateTime CreatedTime { get; set; }
    public bool IsActive { get; set; }

}
