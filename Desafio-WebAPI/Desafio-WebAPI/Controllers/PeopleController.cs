using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/people")]
public class PeopleController(IPeopleService peopleService) : ControllerBase
{
    private readonly IPeopleService _peopleService = peopleService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var result = await _peopleService.GetAll(userId);

        return Ok(new ApiResponse<IEnumerable<PeopleResponse>>(true, "Lista de pessoas retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet("{peopleId}")]
    public async Task<IActionResult> GetById(Guid peopleId)
    {
        var _ = User.GetUserId();
        var result = await _peopleService.GetPeopleById(peopleId);
        return Ok(new ApiResponse<PeopleResponse>(true, "Pessoa retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPeoplePagination([FromQuery] PeopleFilterRequest request)
    {
        var result = await _peopleService.GetPeoplePagination(request);
        return Ok(new ApiResponse<PeoplePaginationResponse>(true, "Lista de pessoas paginada retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PeopleResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(PeopleRequest request)
    {
        var userId = User.GetUserId();

        var result = await _peopleService.CreatePeople(userId, request);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<PeopleResponse>(
            true,
            "Nova pessoa cadastrada com sucesso",
            result.Data
        ));
    }

    [Authorize]
    [HttpPut("{peopleId}")]
    public async Task<IActionResult> Update(Guid peopleId, PeopleRequest request)
    {
        try
        {
            var userId = User.GetUserId();

            var result = await _peopleService.UpdatePeople(userId, peopleId, request);

            if (!result.Success)
                return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

            return Ok(new ApiResponse<PeopleResponse>(true, "Informações alteradas com sucesso", result.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PeopleResponse>(false, $"Bad Request: {ex.Message}"));
        }
    }

    [Authorize]
    [HttpDelete("{peopleId}")]
    public async Task<IActionResult> Delete(Guid peopleId)
    {
        var userId = User.GetUserId();

        var result = await _peopleService.DeletePeople(userId, peopleId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Pessoa deletada com sucesso", true));
    }

}
