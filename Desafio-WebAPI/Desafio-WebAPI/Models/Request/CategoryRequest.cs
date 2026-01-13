using Desafio_WebAPI.Models.Entities;

namespace Desafio_WebAPI.Models.Request;

public class CategoryRequest
{
    public string Description { get; set; } = null!;
    public CategoryType CategoryType { get; set; }
    public bool IsActive { get; set; }
}
