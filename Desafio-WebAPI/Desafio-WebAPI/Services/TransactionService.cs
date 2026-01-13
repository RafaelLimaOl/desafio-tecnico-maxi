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
        // Validação caso PeopleId seja nulo
        if (peopleId == Guid.Empty)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail("Id inválido");

        // Validação para verificar se a Pessoa existe
        var validPeople = await ValidatePeople<AnyType>(peopleId);

        if (!validPeople.Success)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail(validPeople.Message!);

        // Retorno dos dados
        var result = await _transactionRespository.GetAllByPeopleId(peopleId);
        
        // Caso não exista data
        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TransactionResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<TransactionResponse>>.Ok(result);
    }
    
    public async Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByCategory(Guid categoryId)
    {
        // Validação caso CategoryId seja nulo
        if (categoryId == Guid.Empty)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail("Id inválido");
        // Validação caso a Categoria não exista
        var validCategory = await ValidateCategory<AnyType>(categoryId);

        if (!validCategory.Success)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail(validCategory.Message!);

        // Retorno dos dados
        var result = await _transactionRespository.GetAllByCategoryId(categoryId);

       // Caso não exista data
        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TransactionResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<TransactionResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<IEnumerable<TransactionResponse>>> GetAllByUser(Guid userId)
    {
        // Validação se UserId é valido 
        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail("Id inválido");

        var validUser = await ValidateUser<AnyType>(userId);

        if (!validUser.Success)
            return ServiceResponse<IEnumerable<TransactionResponse>>.Fail(validUser.Message!);

        // Retorne a lista de transação por usuário:
        var result = await _transactionRespository.GetAllByUserId(userId);

        // Caso result não possua data 
        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TransactionResponse>>.Ok("Sem data");

        // Retorne Ok e o resultado
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
        // Validação se UserId é valido 
        if (userId == Guid.Empty)
            return ServiceResponse<TransactionResponse>.Fail("Id inválido");

        // Utilização do Fluent Validation para os valores da requisição
        var validator = new TransactionRequestValidator();
        var validationResult = validator.Validate(request);

        // Caso qualquer valor esteja inválido retorne um erro
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<TransactionResponse>.Fail($"Valores inválidos: {errors}");
        }

        // Validação se PeopleId e CategoryId existem 
        var validPeople = await ValidatePeople<PeopleResponse?>(request.PeopleId);
        var validCategory = await ValidateCategory<CategoryResponse?>(request.CategoryId);

        // Caso não sejam válidos retorna Erro:
        if (!validCategory.Success)
            return ServiceResponse<TransactionResponse>.Fail(validCategory.Message!);
        if (!validPeople.Success)
            return ServiceResponse<TransactionResponse>.Fail(validPeople.Message!);

        // Seleceção da cetegoria com o Id fornecido
        var category = await _categoryRepository.GetCategoryById(request.CategoryId);

        // Tipo da Categoria selecionado
        var allowedCategory = category!.CategoryType;

        // Validação do Tipo da categoria com o tipo da transação passada
        // Não será permitido que categorias aceitem valores que não includam suas designadas transações: 
        // Por exemplo: Categoria que aceita Receita não aceitará Transações para Despesas
        // AMBAS aceita Despesa e Receita | Receita apenas aceita Receita | Despesa apenas aceita Despesa
        if (allowedCategory != CategoryType.AMBAS && (TransactionType)allowedCategory != request.TransactionType)
            return ServiceResponse<TransactionResponse>.Fail(
                $"Essa categoria é do tipo {allowedCategory} e não aceita transações do tipo {request.TransactionType}."
            );
        
        // Seleção da pessoa com o Id passado
        var people = await _peopleRepository.GetPeopleById(request.PeopleId);

        // Validação da Idade do usuário menores de Idade não podem ter Receitas:
        if (people!.Age < 18 && request.TransactionType != TransactionType.DESPESA)
            return ServiceResponse<TransactionResponse>.Fail("Pessoas com menos de 18 anos só podem realizar DESPESAS");

        // Execução da criação da nova transação
        var newTransaction = await _transactionRespository.CreateTransaction(userId, request);

        // Transação retornada com os valores passados:
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
        // Validação se UserId é valido 
        if (userId == Guid.Empty)
            return ServiceResponse<TransactionResponse>.Fail("Id inválido");

        // Validação dos valores passados pela requisição

        var validator = new TransactionRequestValidator();
        var validationResult = validator.Validate(request);

        // Caso qualquer valor esteja inválido retorne um erro
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<TransactionResponse>.Fail($"Valores inválidos: {errors}");
        }

        // Validação se PeopleId e CategoryId existem 

        var validPeople = await ValidatePeople<PeopleResponse?>(request.PeopleId);
        var validCategory = await ValidateCategory<CategoryResponse?>(request.CategoryId);

        if (!validCategory.Success)
            return ServiceResponse<TransactionResponse>.Fail(validCategory.Message!);
        if (!validPeople.Success)
            return ServiceResponse<TransactionResponse>.Fail(validPeople.Message!);

        var category = await _categoryRepository.GetCategoryById(request.CategoryId);

        // AMBAS aceita Despesa e Receita | Receita apenas aceita Receita | Despesa apenas aceita Despesa
        var allowedCategory = category!.CategoryType;

        if (allowedCategory != CategoryType.AMBAS && (TransactionType)allowedCategory != request.TransactionType)
            return ServiceResponse<TransactionResponse>.Fail(
                $"Essa categoria é do tipo {allowedCategory} e não aceita transações do tipo {request.TransactionType}."
            );

        // Validação da Idade do usuário menores de Idade não podem ter Receitas:
        var people = await _peopleRepository.GetPeopleById(request.PeopleId);
        if (people!.Age < 18 && request.TransactionType != TransactionType.DESPESA)
            return ServiceResponse<TransactionResponse>.Fail("Pessoas com menos de 18 anos só podem realizar DESPESAS");

        // Atualização da transação com os novos dados
        var newTransaction = await _transactionRespository.UpdateTransaction(userId, transactionId, request);

        // Seleção dos novos valores com a transação pelo Id passado
        var updatedCategory = await _transactionRespository.GetTransactionById(transactionId);

        // Transação retornada com os valores passados:
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
        // Validação caso UserId e TransaçãoId não são nulos
        if (userId == Guid.Empty || transactionId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id do usuário ou pessoa inválido");

        // Verificação caso Usuário exista com Id passado
        var validUser = await ValidateUser<PeopleRequest>(userId);

        // Caso
        if (!validUser.Success)
            return ServiceResponse<bool>.Fail(validUser.Message!);

        // Validação caso transação existe e se a transação passada pertence ao usuário passado
        var existTransaction = await _transactionRespository.ExistTransaction(transactionId);
        var transactionContainsInUser = await _transactionRespository.TransactionContainsInUser(transactionId, userId);

        // Retorno de um erro caso não exista
        if (!existTransaction || !transactionContainsInUser)
            return ServiceResponse<bool>.Fail($"Transação não encontrada com o Id fornecdio: {transactionId}");

        // Deletar transação
        var result = await _transactionRespository.DeleteTransaction(transactionId);
        // Retorne um erro caso alguma coisa aconteça
        if (!result)
            return ServiceResponse<bool>.Fail("Transação com o Id fornecido não foi deletada");

        // Retorne Ok
        return ServiceResponse<bool>.Ok("Transação deletada com sucesso");
    }

    // Métodos privados:

    private async Task<ServiceResponse<T>> ValidateUser<T>(Guid userId)
    {
        // Validação para procurar um usuário com o Id passado
        var existUser = await _userRepository.ExistUser(userId);

        if (!existUser)
            return ServiceResponse<T>.Fail("Usuário inválido");

        return ServiceResponse<T>.Ok("Usuário válido");
    }
    private async Task<ServiceResponse<T>> ValidatePeople<T>(Guid peopleId)
    {
        // Validação para procurar uma pessoa com o Id passado
        var existPeople = await _peopleRepository.GetPeopleById(peopleId);

        if (existPeople == null)
            return ServiceResponse<T>.Fail("Pessoa não encontrada");

        return ServiceResponse<T>.Ok("Pessoa válida");
    }
    private async Task<ServiceResponse<T>> ValidateCategory<T>(Guid categoryId)
    {
        // Validação para procurar uma categoria com o Id passado
        var existCategory = await _categoryRepository.GetCategoryById(categoryId);

        if (existCategory == null)
            return ServiceResponse<T>.Fail("Categoria não encontrada");

        return ServiceResponse<T>.Ok("Categoria válida");
    }
}
