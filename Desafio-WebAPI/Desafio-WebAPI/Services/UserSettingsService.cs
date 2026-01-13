using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;

namespace Desafio_WebAPI.Services;

public class UserSettingsService(IUserRepository userRepository) : IUserSettingsService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ServiceResponse<UserSettingsResponse>> GetUserSettings(Guid userId)
    {
        if (userId == Guid.Empty)
            return ServiceResponse<UserSettingsResponse>.Fail("Id inválido");

        var result = await _userRepository.GetUserSettings(userId);

        if (result is null)
            return ServiceResponse<UserSettingsResponse>.Ok("Sem data");

        return ServiceResponse<UserSettingsResponse>.Ok(result);
    }

    public async Task<ServiceResponse<UserSettingsResponse>> UpdateUserSettings(Guid userId, UserSettingsRequest request)
    {
        var validUser = await ValidateUser<PeopleRequest>(userId);

        if (!validUser.Success)
            return ServiceResponse<UserSettingsResponse>.Fail("Usuário inválidao");

        var updated = await _userRepository.EditUserSettings(userId, request);

        if (!updated)
            return ServiceResponse<UserSettingsResponse>.Fail("Pessoa não encontrada");

        var updatedUser = await _userRepository.GetUserSettings(userId);

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
        if (userId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id inválido");

        var validUser = await ValidateUser<UserSettingsResponse>(userId);

        if (!validUser.Success)
            return ServiceResponse<bool>.Fail(validUser.Message!);

        var result = await _userRepository.DeleteUser(userId);
        if (!result)
            return ServiceResponse<bool>.Fail("Conta com o Id fornecido não foi deletada");

        return ServiceResponse<bool>.Ok("Conta deletada com sucesso");
    }


    public async Task<ServiceResponse<T>> ValidateUser<T>(Guid userId)
    {
        var existUser = await _userRepository.ExistUser(userId);

        if (!existUser)
            return ServiceResponse<T>.Fail("Usuário inválido");

        return ServiceResponse<T>.Ok("Usuário válido");

    }
}
