using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Controllers;

[ApiController]
[Route("api/settings")]
public class UserSettingsController(IUserSettingsService userSettings) : ControllerBase
{
    private readonly IUserSettingsService _userSettings = userSettings;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserSettings()
    {
        var userId = User.GetUserId();

        var result = await _userSettings.GetUserSettings(userId);

        return Ok(new ApiResponse<UserSettingsResponse>(true, "Configurações retornadas com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(UserSettingsRequest request)
    {
        try
        {
            var userId = User.GetUserId();

            var result = await _userSettings.UpdateUserSettings(userId, request);

            if (!result.Success)
                return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

            return Ok(new ApiResponse<UserSettingsResponse>(true, "Configurações alteradas com sucesso", result.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserSettingsResponse>(false, $"Bad Request: {ex.Message}"));
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = User.GetUserId();

        var result = await _userSettings.DeleteUser(userId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Usuário deletado com sucesso!", true));
    }
}
