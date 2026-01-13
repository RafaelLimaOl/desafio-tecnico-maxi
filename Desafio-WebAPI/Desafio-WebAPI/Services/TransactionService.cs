using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Entities;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Models.Validators;
using Microsoft.OpenApi.Any;

namespace Desafio_WebAPI.Services;

public class TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, IPeopleRepository peopleRepository, ICategoryRepository categoryRepository) : ITransactionService
{
    private readonly ITransactionRepository _transactionRespository = transactionRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPeopleRepository _peopleRepository = peopleRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByPeople(Guid peopleId)
    {

        if (peopleId == Guid.Empty)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail("Id inválido");

        var validPeople = await ValidatePeople<AnyType>(peopleId);

        if (!validPeople.Success)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail(validPeople.Message!);

        var result = await _transactionRespository.GetAllByPeopleId(peopleId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TransactionResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<TransactionResponse>>.Ok(result);
    }
    public async Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByCategory(Guid categoryId)
    {

        if (categoryId == Guid.Empty)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail("Id inválido");

        var validCategory = await ValidateCategory<AnyType>(categoryId);

        if (!validCategory.Success)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail(validCategory.Message!);

        var result = await _transactionRespository.GetAllByCategoryId(categoryId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TransactionResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<TransactionResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByUser(Guid userId)
    {

        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail("Id inválido");

        var validUser = await ValidateUser<AnyType>(userId);

        if (!validUser.Success)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail(validUser.Message!);

        var result = await _transactionRespository.GetAllByUserId(userId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TransactionResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<TransactionResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<TransactionResponse>> GetTransactionById(Guid transactionId)
    {
        if (transactionId == Guid.Empty)
            return ServiceResponse<TransactionResponse>.Fail("Id inválido");

        var existTransaction = await _transactionRespository.ExistTransaction(transactionId);
        
        if (!existTransaction)
            return ServiceResponse<TransactionResponse>.Fail($"Pessoa não encontrada com o Id fornecido: {transactionId}");

        var result = await _transactionRespository.GetTransactionById(transactionId);
        if (result is null)
            return ServiceResponse<TransactionResponse>.Ok("Sem data");

        return ServiceResponse<TransactionResponse>.Ok(result);
    }

    public async Task<ServiceResponse<TransactionResponse>> CreateTransaction(Guid userId, TransactionRequest request)
    {
        var validator = new TransactionRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<TransactionResponse>.Fail($"Valores inválidos: {errors}");
        }

        var validPeople = await ValidatePeople<PeopleResponse?>(request.PeopleId);
        var validCategory = await ValidateCategory<CategoryResponse?>(request.CategoryId);

        if (!validCategory.Success)
            return ServiceResponse<TransactionResponse>.Fail(validCategory.Message!);
        if (!validPeople.Success)
            return ServiceResponse<TransactionResponse>.Fail(validPeople.Message!);

        var category = await _categoryRepository.GetCategoryById(request.CategoryId);

        var allowedCategory = category!.CategoryType;

        if (allowedCategory != CategoryType.AMBAS && (TransactionType)allowedCategory != request.TransactionType)
            return ServiceResponse<TransactionResponse>.Fail(
                $"Essa categoria é do tipo {allowedCategory} e não aceita transações do tipo {request.TransactionType}."
            );
        
        var people = await _peopleRepository.GetPeopleById(request.PeopleId);
        if (people!.Age < 18 && request.TransactionType != TransactionType.DESPESA)
            return ServiceResponse<TransactionResponse>.Fail("Pessoas com menos de 18 anos só podem realizar DESPESAS");

        var newTransaction = await _transactionRespository.CreateTransaction(userId, request);

        return ServiceResponse<TransactionResponse>.Ok(new TransactionResponse
        {
            Id = newTransaction.Id,
            Description = newTransaction.Description,
            Amount = newTransaction.Amount,
            CategoryId = newTransaction.CategoryId,
            PeopleId = newTransaction.PeopleId,
            UserId = newTransaction.UserId,
            TransactionType = newTransaction.TransactionType,
            Status = newTransaction.Status,
            IsActive = newTransaction.IsActive,
        });
    }

    public async Task<ServiceResponse<TransactionResponse>> UpdateTransaction(Guid userId, Guid transactionId, TransactionRequest request)
    {
        var validator = new TransactionRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<TransactionResponse>.Fail($"Valores inválidos: {errors}");
        }

        var validPeople = await ValidatePeople<PeopleResponse?>(request.PeopleId);
        var validCategory = await ValidateCategory<CategoryResponse?>(request.CategoryId);

        if (!validCategory.Success)
            return ServiceResponse<TransactionResponse>.Fail(validCategory.Message!);
        if (!validPeople.Success)
            return ServiceResponse<TransactionResponse>.Fail(validPeople.Message!);

        var category = await _categoryRepository.GetCategoryById(request.CategoryId);

        var allowedCategory = category!.CategoryType;

        if (allowedCategory != CategoryType.AMBAS && (TransactionType)allowedCategory != request.TransactionType)
            return ServiceResponse<TransactionResponse>.Fail(
                $"Essa categoria é do tipo {allowedCategory} e não aceita transações do tipo {request.TransactionType}."
            );

        var people = await _peopleRepository.GetPeopleById(request.PeopleId);
        if (people!.Age < 18 && request.TransactionType != TransactionType.DESPESA)
            return ServiceResponse<TransactionResponse>.Fail("Pessoas com menos de 18 anos só podem realizar DESPESAS");

        var newTransaction = await _transactionRespository.UpdateTransaction(userId, transactionId, request);

        var updatedCategory = await _transactionRespository.GetTransactionById(transactionId);

        return ServiceResponse<TransactionResponse>.Ok(new TransactionResponse
        {
            Id = updatedCategory!.Id,
            Description = updatedCategory.Description,
            Amount = updatedCategory.Amount,
            CategoryId = updatedCategory.CategoryId,
            PeopleId = updatedCategory.PeopleId,
            UserId = updatedCategory.UserId,
            TransactionType = updatedCategory.TransactionType,
            Status = updatedCategory.Status,
            IsActive = updatedCategory.IsActive,
        });
    }

    public async Task<ServiceResponse<bool>> DeleteTransaction(Guid userId, Guid transactionId)
    {
        if (userId == Guid.Empty || transactionId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id do usuário ou pessoa inválido");

        var validUser = await ValidateUser<PeopleRequest>(userId);

        if (!validUser.Success)
            return ServiceResponse<bool>.Fail(validUser.Message!);

        var existTransaction = await _transactionRespository.ExistTransaction(transactionId);
        var transactionContainsInUser = await _transactionRespository.TransactionContainsInUser(transactionId, userId);

        if (!existTransaction || !transactionContainsInUser)
            return ServiceResponse<bool>.Fail($"Transação não encontrada com o Id fornecdio: {transactionId}");

        var result = await _transactionRespository.DeleteTransaction(transactionId);
        if (!result)
            return ServiceResponse<bool>.Fail("Transação com o Id fornecido não foi deletada");

        return ServiceResponse<bool>.Ok("Transação deletada com sucesso");
    }

    // Métodos privados:

    private async Task<ServiceResponse<T>> ValidateUser<T>(Guid userId)
    {
        var existUser = await _userRepository.ExistUser(userId);

        if (!existUser)
            return ServiceResponse<T>.Fail("Usuário inválido");

        return ServiceResponse<T>.Ok("Usuário válido");
    }
    private async Task<ServiceResponse<T>> ValidatePeople<T>(Guid peopleId)
    {
        var existPeople = await _peopleRepository.GetPeopleById(peopleId);

        if (existPeople == null)
            return ServiceResponse<T>.Fail("Pessoa não encontrada");

        return ServiceResponse<T>.Ok("Pessoa válida");
    }
    private async Task<ServiceResponse<T>> ValidateCategory<T>(Guid categoryId)
    {
        var existCategory = await _categoryRepository.GetCategoryById(categoryId);

        if (existCategory == null)
            return ServiceResponse<T>.Fail("Categoria não encontrada");

        return ServiceResponse<T>.Ok("Categoria válida");
    }
}
