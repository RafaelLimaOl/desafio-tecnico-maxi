using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Models.Validators;

namespace Desafio_WebAPI.Services;

public class PeopleService(IPeopleRepository peopleRepository, IUserRepository userRepository) : IPeopleService
{
    private readonly IPeopleRepository _peopleRepository = peopleRepository;
    private readonly IUserRepository _userRepository = userRepository;

    // Variáveis fixas para a páginação
    private const int MAX_LIMIT = 100;
    private const int DEFAULT_LIMIT = 10;
    private static readonly HashSet<string> AllowedSortColumns =
    [
        "Name", "Age", "CreatedTime", "Id"
    ];

    public async Task<ServiceResponse<IEnumerable<PeopleResponse>>> GetAll(Guid userId)
    {
        // Validação caso UserId seja nulo
        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<PeopleResponse>>.Fail("Id inválido");
        
        // Verificação se Usuário existe
        var validUser = await ValidateUser<PeopleRequest>(userId);

        if (!validUser.Success)
            return ServiceResponse<IEnumerable<PeopleResponse>>.Fail(validUser.Message!);

        // Retorno dos dados
        var result = await _peopleRepository.GetAllPeople(userId);

        // Caso não exista data
        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<PeopleResponse>>.Ok("Sem data");

        return ServiceResponse<IEnumerable<PeopleResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<PeopleResponse>> GetPeopleById(Guid peopleId)
    {
        // Validação caso PeopleId seja nulo
        if (peopleId == Guid.Empty)
            return ServiceResponse<PeopleResponse>.Fail("Id inválido");
        var existPeople = await _peopleRepository.ExistPeople(peopleId);

        // Verificação se a Pessoa existe
        if (!existPeople)
            return ServiceResponse<PeopleResponse>.Fail($"Pessoa não encontrada com o Id fornecido: {peopleId}");

        // Retorno dos dados
        var result = await _peopleRepository.GetPeopleById(peopleId);
        // Caso não exista data
        if (result is null)
            return ServiceResponse<PeopleResponse>.Ok("Sem data");

        return ServiceResponse<PeopleResponse>.Ok(result);
    }

    // Função de paginação
    // INFO: Não usada no desafio, apenas criada para futura implementação do front
    public async Task<ServiceResponse<PeoplePaginationResponse>> GetPeoplePagination(PeopleFilterRequest request)
    {
        request.Offset = request.Offset < 0 ? 0 : request.Offset;
        request.Limit = request.Limit <= 0 ? DEFAULT_LIMIT : Math.Min(request.Limit, MAX_LIMIT);

        request.Sort = AllowedSortColumns.Contains(request.Sort) ? request.Sort : "Id";

        request.Order = request.Order.Equals("DESC", StringComparison.OrdinalIgnoreCase)
            ? "DESC"
            : "ASC";

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            request.SearchTerm = request.SearchTerm.Trim();

        var (totalRecords, peoples) = await _peopleRepository.GetPeoplePagination(request);

        int currentPage = (request.Offset / request.Limit) + 1;
        int totalPages = (int)Math.Ceiling(totalRecords / (double)request.Limit);

        return new ServiceResponse<PeoplePaginationResponse>
        {
            Success = true,
            Message = "Lista de pessoas retornada com sucesso" ,
            Data = new PeoplePaginationResponse
            {
                Peoples = peoples,
                CurrentPage = currentPage,
                TotalPages = totalPages
            }
        };
    }

    public async Task<ServiceResponse<PeopleResponse>> CreatePeople(Guid userId, PeopleRequest request)
    {
        // Validação se UserId é valido 
        if (userId == Guid.Empty)
            return ServiceResponse<PeopleResponse>.Fail("Id inválido");

        // Utilização do Fluent Validation para os valores da requisição
        var validator = new PeopleRequestValidator();
        var validationResult = validator.Validate(request);
        
        // Caso qualquer valor esteja inválido retorne um erro
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<PeopleResponse>.Fail($"Valores inválidos: {errors}");
        }

        // Verificação se o Usuário existe
        var validUser = await _userRepository.ExistUser(userId);

        if (validUser == false)
            return ServiceResponse<PeopleResponse>.Fail("Usuário inválido");

        // Execução da criação da nova pessoa
        var newPeople = await _peopleRepository.CreatePeople(userId, request);

        // Retorno dos dados criados
        return ServiceResponse<PeopleResponse>.Ok(new PeopleResponse
        {
            Id = newPeople.Id,
            Name = newPeople.Name,
            Age = newPeople.Age,
            IsActive = newPeople.IsActive,
        });
    }
    
    public async Task<ServiceResponse<PeopleResponse>> UpdatePeople(Guid userId, Guid peopleId, PeopleRequest request)
    {
        // Validação se UserId é valido 
        if (userId == Guid.Empty)
            return ServiceResponse<PeopleResponse>.Fail("Id inválido");

        // Utilização do Fluent Validation para os valores da requisição
        var validator = new PeopleRequestValidator();
        var validationResult = validator.Validate(request);

        // Caso qualquer valor esteja inválido retorne um erro
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ServiceResponse<PeopleResponse>.Fail($"Valores inválidos: {errors}");
        }

        // Validação se a pessoa existe e pertence ao usuário 
        var validUser = await ValidateUser<PeopleRequest>(userId);
        var existPeople = await _peopleRepository.GetPeopleById(peopleId);

        if (existPeople is null || !validUser.Success)
            return ServiceResponse<PeopleResponse>.Fail("Usuário ou Pessoa inválida");

        if (peopleId == Guid.Empty)
            return ServiceResponse<PeopleResponse>.Fail("Id inválido");

        // Execução da atualização das informações
        var updated = await _peopleRepository.UpdatePeople(userId, peopleId, request);

        if (!updated)
            return ServiceResponse<PeopleResponse>.Fail("Não foi possível atualizar os dados");

        // Seleção da pessoa com as novas informações
        var updatedUser = await _peopleRepository.GetPeopleById(peopleId);

        // Pessoa retornada com os valores passados:
        return ServiceResponse<PeopleResponse>.Ok(new PeopleResponse
        {
            Id = updatedUser!.Id,
            Name = updatedUser.Name,
            Age = updatedUser.Age,
            IsActive = updatedUser.IsActive,
        });
    }

    public async Task<ServiceResponse<bool>> DeletePeople(Guid userId, Guid peopleId)
    {
        // Validação caso UserId ou PeopleId passados sejam nulos
        if (userId == Guid.Empty || peopleId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id do usuário ou pessoa inválido");

        // Validação caso usuário exista
        var validUser = await ValidateUser<PeopleRequest>(userId);

        if (!validUser.Success)
            return ServiceResponse<bool>.Fail(validUser.Message!);

        // Validação da pessoa, junto com a validação se pessoa Pertence ao usuário
        var existPeople = await _peopleRepository.ExistPeople(peopleId);
        var peopleContainsInUser = await _peopleRepository.PeopleContainsInUser(peopleId, userId);

        // Caso a pessoa não exista ou não pertença será retorna um erro:
        if (!existPeople || !peopleContainsInUser)
            return ServiceResponse<bool>.Fail($"Pessoa não encontrada com o Id fornecdio: {peopleId}");

        // Execução do Delete
        var result = await _peopleRepository.DeletePeople(peopleId);
        // Caso algum erro ocorra
        if (!result)
            return ServiceResponse<bool>.Fail("Pessoa com o Id fornecido não foi deletada");

        return ServiceResponse<bool>.Ok("Pessoa deletada com sucesso");
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

}
