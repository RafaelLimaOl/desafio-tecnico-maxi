using Desafio_WebAPI.Models.Entities;

namespace Desafio_WebAPI.Models.Response;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public CategoryType CategoryType { get; set; }
    public bool IsActive { get; set; }
}
