using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Models.Request;
using Desafio_WebAPI.Models.Response;
using Desafio_WebAPI.Models.Response.Wrapper;
using Desafio_WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    [Authorize]
    [HttpGet("by-user")]
    public async Task<IActionResult> GetAllByUser()
    {
        var userId = User.GetUserId();
        var result = await _transactionService.GetAllByUser(userId);

        return Ok(new ApiResponse<IEnumerable<TransactionResponse>>(true, "Lista de transações retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet("by-people")]
    public async Task<IActionResult> GetAllByPeople(Guid peopleId)
    {
        var result = await _transactionService.GetAllByPeople(peopleId);

        return Ok(new ApiResponse<IEnumerable<TransactionResponse>>(true, "Lista de transações retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet("by-category")]
    public async Task<IActionResult> GetAllByCategory(Guid categoryId)
    {
        var result = await _transactionService.GetAllByPeople(categoryId);

        return Ok(new ApiResponse<IEnumerable<TransactionResponse>>(true, "Lista de transações retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetById(Guid transactionId)
    {
        var result = await _transactionService.GetTransactionById(transactionId);
        return Ok(new ApiResponse<TransactionResponse>(true, "Transação retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(TransactionRequest newTransaction)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _transactionService.CreateTransaction(userId, newTransaction);

            if (!result.Success)
                return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

            return Ok(new ApiResponse<TransactionResponse>(true, "Nova transação cadastrada com sucesso", result.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TransactionResponse>(false, $"Bad Request: {ex.Message}"));
        }
    }

    [Authorize]
    [HttpPut("{transactionId}")]
    public async Task<IActionResult> Update(Guid transactionId, TransactionRequest request)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _transactionService.UpdateTransaction(userId, transactionId, request);

            if (!result.Success)
                return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

            return Ok(new ApiResponse<TransactionResponse>(true, "Transação editada com sucesso", result.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TransactionResponse>(false, $"Bad Request: {ex.Message}"));
        }
    }

    [Authorize]
    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> Delete(Guid transactionId)
    {
        var userId = User.GetUserId();
        var result = await _transactionService.DeleteTransaction(userId, transactionId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Transação deletada com sucesso", true));
    }
}
