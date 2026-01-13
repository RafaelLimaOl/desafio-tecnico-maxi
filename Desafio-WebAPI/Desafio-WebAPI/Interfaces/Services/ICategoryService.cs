using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;

namespace Desafio_WebAPI.Interfaces.Services;

public interface ICategoryService
{
    Task<ServiceResponse<IEnumerable<CategoryResponse>>> GetAll(Guid userId);
    Task<ServiceResponse<CategoryResponse>> GetCategoryById(Guid categoryId);
    Task<ServiceResponse<CategoryResponse>> CreateCategory(Guid userId, CategoryRequest request);
    Task<ServiceResponse<CategoryResponse>> UpdateCategory(Guid userId, Guid categoryId, CategoryRequest request);
    Task<ServiceResponse<bool>> DeleteCategory(Guid userId, Guid categoryId);

}
