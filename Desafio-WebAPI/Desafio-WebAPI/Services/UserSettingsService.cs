using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;

namespace Desafio_WebAPI.Services;

public class UserSettingsService(IUserRepository userRepository) : IUserSettingsService
{
    private readonly IUserRepository _userRepository = userRepository;

    // Função para validar a busca por Configurações
    public async Task<ServiceResponse<UserSettingsResponse>> GetUserSettings(Guid userId)
    {
        // Verificação se UserId passado não é nulo
        if (userId == Guid.Empty)
            return ServiceResponse<UserSettingsResponse>.Fail("Id inválido");

        // Verificar se existe o usuário com o Id passado
        var result = await _userRepository.GetUserSettings(userId);

        // Caso não exista retorne Sem Data, pois já foi validado se o userId é válido
        if (result is null)
            return ServiceResponse<UserSettingsResponse>.Ok("Sem data");

        // Retorne as informações
        return ServiceResponse<UserSettingsResponse>.Ok(result);
    }

    public async Task<ServiceResponse<UserSettingsResponse>> UpdateUserSettings(Guid userId, UserSettingsRequest request)
    {
        // Verifica se existe usuário com o Id passado
        var validUser = await ValidateUser<PeopleRequest>(userId);

        // Caso algum erro aconteça não retorne usuário inválido
        if (!validUser.Success)
            return ServiceResponse<UserSettingsResponse>.Fail("Usuário inválidao");

        // Execução da edição dos dados
        var updated = await _userRepository.EditUserSettings(userId, request);

        // Validação de algum erro retornado
        if (!updated)
            return ServiceResponse<UserSettingsResponse>.Fail("Erro ao editar as informações");

        // Função para retornar os dados atualizados
        var updatedUser = await _userRepository.GetUserSettings(userId);

        // Retorno Ok com os dados atualizados do usuário
        return ServiceResponse<UserSettingsResponse>.Ok(new UserSettingsResponse
        {
            Id = updatedUser!.Id,
            UserName = updatedUser!.UserName,
            Email = updatedUser.Email,
            IsActive = updatedUser.IsActive,
        });
    }

    public async Task<ServiceResponse<bool>> DeleteUser(Guid userId)
    {
        // Verificação se UserId passado não é nulo
        if (userId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id inválido");

        // Validação se o usuário existe
        var validUser = await ValidateUser<UserSettingsResponse>(userId);

        // Retorno caso usuário não seja encontrado
        if (!validUser.Success)
            return ServiceResponse<bool>.Fail(validUser.Message!);

        // Execução do deletamento do usuário
        var result = await _userRepository.DeleteUser(userId);

        // Validação caso não seja deletado a conta
        if (!result)
            return ServiceResponse<bool>.Fail("Conta com o Id fornecido não foi deletada");

        // Retorno Ok usuário deletado
        return ServiceResponse<bool>.Ok("Conta deletada com sucesso");
    }


    public async Task<ServiceResponse<T>> ValidateUser<T>(Guid userId)
    {
        // Selecionar usuário por Id
        var existUser = await _userRepository.ExistUser(userId);

        // Caso não exista, retorne erro
        if (!existUser)
            return ServiceResponse<T>.Fail("Usuário inválido");

        // Retorne resultado OK Usuário váldio
        return ServiceResponse<T>.Ok("Usuário válido");
    }
}
