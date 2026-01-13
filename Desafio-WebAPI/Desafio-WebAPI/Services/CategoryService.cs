using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Models.Validators;

namespace Desafio_WebAPI.Services;

public class CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ServiceResponse<IEnumerable<CategoryResponse>>> GetAll(Guid userId)
    {
        // Validação caso UserId seja nulo
        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<CategoryResponse>>.Fail("Id inválido");

        var validUser = await ValidateUser<PeopleRequest>(userId);

        // Verificação se o Usuário existe
        if (!validUser.Success)
            return ServiceResponse<IEnumerable<CategoryResponse>>.Fail(validUser.Message!);

         // Retorno dos dados
        var result = await _categoryRepository.GetAllCategories(userId);
        // Caso não exista data
        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<CategoryResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<CategoryResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<CategoryResponse>> GetCategoryById(Guid categoryId)
    {
        // Validação caso PeopleId seja nulo
        if (categoryId == Guid.Empty)
            return ServiceResponse<CategoryResponse>.Fail("Id inválido");
        var existCategory = await _categoryRepository.ExistCategory(categoryId);

        // Verificação se a Categoria existe
        if (!existCategory)
            return ServiceResponse<CategoryResponse>.Fail($"Categoria não encontrada com o Id fornecido: {categoryId}");

        // Retorno dos dados
        var result = await _categoryRepository.GetCategoryById(categoryId);
        // Caso não exista data
        if (result is null)
            return ServiceResponse<CategoryResponse>.Ok("Sem data");

        return ServiceResponse<CategoryResponse>.Ok(result);
    }

    public async Task<ServiceResponse<CategoryResponse>> CreateCategory(Guid userId, CategoryRequest request)
    {
        // Validação caso UserId seja nulo
        if (userId == Guid.Empty)
            return ServiceResponse<CategoryResponse>.Fail("Id inválido");

        // Utilização do Fluent Validation para os valores da requisição
        var validator = new CategoryRequestValidator();
        var validationResult = validator.Validate(request);

        // Caso qualquer valor esteja inválido retorne um erro
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<CategoryResponse>.Fail($"Valores inválidos: {errors}");
        }

        // Verificação se o Usuário existe
        var validUser = await _userRepository.ExistUser(userId);

        if (validUser == false)
            return ServiceResponse<CategoryResponse>.Fail("Usuário inválido");

        // Execução da criação da nova categoria
        var newCategory = await _categoryRepository.CreateCategory(userId, request);

        // Retorno dos dados criados
        return ServiceResponse<CategoryResponse>.Ok(new CategoryResponse
        {
            Id = newCategory.Id,
            Description = newCategory.Description,
            CategoryType = newCategory.CategoryType,
            IsActive = newCategory.IsActive,
        });
    }

    public async Task<ServiceResponse<CategoryResponse>> UpdateCategory(Guid userId, Guid categoryId, CategoryRequest request)
    {

        // Validação caso UserId seja nulo
        if (userId == Guid.Empty)
            return ServiceResponse<CategoryResponse>.Fail("Id inválido");

        // Utilização do Fluent Validation para os valores da requisição
        var validator = new CategoryRequestValidator();
        var validationResult = validator.Validate(request);

        // Caso qualquer valor esteja inválido retorne um erro
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<CategoryResponse>.Fail($"Valores inválidos: {errors}");
        }

        // Verificação se a Categoria existe
        if (categoryId == Guid.Empty)
            return ServiceResponse<CategoryResponse>.Fail("Id inválido");

        // Verificação se o Usuário existe e Categoria pertence ao usuário
        var validUser = await ValidateUser<CategoryResponse>(userId);
        var existCategory = await _categoryRepository.GetCategoryById(categoryId);

        if (existCategory is null || !validUser.Success)
            return ServiceResponse<CategoryResponse>.Fail("Categoria ou Pessoa inválida");

        if (request == null)
            return ServiceResponse<CategoryResponse>.Fail("Request inválido");

        // Execução da criação da nova pessoa
        var updated = await _categoryRepository.UpdateCategory(userId, categoryId, request);

        if (!updated)
            return ServiceResponse<CategoryResponse>.Fail("Categoria não encontrada");

        // Categoria selecionada com as novas informações
        var updatedUser = await _categoryRepository.GetCategoryById(categoryId);

        // Retorno da Categoria com as novas informações
        return ServiceResponse<CategoryResponse>.Ok(new CategoryResponse
        {
            Id = updatedUser!.Id,
            Description = updatedUser.Description,
            CategoryType = updatedUser.CategoryType,
            IsActive = updatedUser.IsActive,
        });
    }

    public async Task<ServiceResponse<bool>> DeleteCategory(Guid userId, Guid categoryId)
    {
        // Validação caso UserId seja nulo
        if (userId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id inválido");

        if (userId == Guid.Empty || categoryId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id do usuário ou categoria inválido");

        var validUser = await ValidateUser<PeopleRequest>(userId);

        if (!validUser.Success)
            return ServiceResponse<bool>.Fail(validUser.Message!);

        var existCategory = await _categoryRepository.ExistCategory(categoryId);
        var categoryContainsInUser = await _categoryRepository.CategoryContainsInUser(categoryId, userId);

        if (!existCategory || !categoryContainsInUser)
            return ServiceResponse<bool>.Fail($"Categoria não encontrada com o Id fornecdio: {categoryId}");

        var result = await _categoryRepository.DeleteCategory(categoryId);
        if (!result)
            return ServiceResponse<bool>.Fail("Categoria com o Id fornecido não foi deletada");

        return ServiceResponse<bool>.Ok("Categoria deletada com sucesso");
    }

    // Métodos privados:

    private async Task<ServiceResponse<T>> ValidateUser<T>(Guid userId)
    {
        // Lógica para validar se o usuário existe 
        var existUser = await _userRepository.ExistUser(userId);

        if (!existUser)
            return ServiceResponse<T>.Fail("Usuário inválido");

        return ServiceResponse<T>.Ok("Usuário válido");

    }
}
