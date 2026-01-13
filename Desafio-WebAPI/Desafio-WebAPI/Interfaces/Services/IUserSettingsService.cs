using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;

namespace Desafio_WebAPI.Interfaces.Services;

public interface IUserSettingsService
{
    Task<ServiceResponse<UserSettingsResponse>> GetUserSettings(Guid userId);
    Task<ServiceResponse<UserSettingsResponse>> UpdateUserSettings(Guid userId, UserSettingsRequest request);
    Task<ServiceResponse<bool>> DeleteUser(Guid userId);
    Task<ServiceResponse<T>> ValidateUser<T>(Guid userId);
}
