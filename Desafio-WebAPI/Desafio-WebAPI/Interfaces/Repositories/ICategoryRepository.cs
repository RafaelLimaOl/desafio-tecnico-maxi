using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;

namespace Desafio_WebAPI.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryResponse>> GetAllCategories(Guid userId);
    Task<CategoryResponse?> GetCategoryById(Guid categoryId);
    Task<CategoryResponse> CreateCategory(Guid userId, CategoryRequest request);
    Task<bool> UpdateCategory(Guid userId, Guid categoryId, CategoryRequest request);
    Task<bool> DeleteCategory(Guid categoryId);
    Task<bool> ExistCategory(Guid categoryId);
    Task<bool> CategoryContainsInUser(Guid categoryId, Guid userId);
}
