using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();

        var result = await _categoryService.GetAll(userId);

        return Ok(new ApiResponse<IEnumerable<CategoryResponse>>(true, "Lista de categorias retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetById(Guid categoryId)
    {
        var result = await _categoryService.GetCategoryById(categoryId);
        return Ok(new ApiResponse<CategoryResponse>(true, "Categoria retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CategoryRequest request)
    {
        var userId = User.GetUserId();

        var result = await _categoryService.CreateCategory(userId, request);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<CategoryResponse>(
            true,
            "Nova categoria cadastrada com sucesso",
            result.Data
        ));
    }

    [Authorize]
    [HttpPut("{categoryId}")]
    public async Task<IActionResult> Update(Guid categoryId, CategoryRequest request)
    {
        try
        {
            var userId = User.GetUserId();

            var result = await _categoryService.UpdateCategory(userId, categoryId, request);

            if (!result.Success)
                return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

            return Ok(new ApiResponse<CategoryResponse>(true, "Informações alteradas com sucesso", result.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<CategoryResponse>(false, $"Bad Request: {ex.Message}"));
        }
    }

    [Authorize]
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        var userId = User.GetUserId();

        var result = await _categoryService.DeleteCategory(userId, categoryId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Categoria deletada com sucesso", true));
    }
}
