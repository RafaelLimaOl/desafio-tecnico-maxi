namespace Desafio_WebAPI.Models.Response;

public class CategoryPaginationResponse
{
    public List<CategoryResponse>? Categories { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
