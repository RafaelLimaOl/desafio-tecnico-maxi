using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;

namespace Desafio_WebAPI.Interfaces.Repositories;

public interface IUserRepository
{
    Task<UserSettingsResponse?> GetUserSettings(Guid userId);
    Task<bool> EditUserSettings(Guid userId, UserSettingsRequest request);
    Task<bool> DeleteUser(Guid userId);
    Task<bool> ExistUser(Guid userId);
}
