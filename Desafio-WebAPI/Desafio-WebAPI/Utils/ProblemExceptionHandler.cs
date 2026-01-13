using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Utils;

[Serializable]
public class ProblemExeption(string error, string message, int statusCode) : Exception(message)
{
    public string Error { get; } = error;
    public string DetailMessage { get; } = message;
    public int StatusCode { get; } = statusCode;
}

public class ProblemExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{

    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ProblemExeption problemExeption)
        {
            return true;
        }

        var problemDetails = new ProblemDetails
        {
            Status = problemExeption.StatusCode,
            Title = problemExeption.Error,
            Detail = problemExeption.Message,
            Type = GetProblemType(problemExeption.StatusCode)
        };

        httpContext.Response.StatusCode = problemExeption.StatusCode;
        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            }
        );
    }

    private static string GetProblemType(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Requisição errada",
        StatusCodes.Status401Unauthorized => "Não autorizado",
        StatusCodes.Status403Forbidden => "Proibido",
        StatusCodes.Status404NotFound => "Não encontrado",
        StatusCodes.Status500InternalServerError => "Erro interno do Server",
        _ => "Erro desconhecido"
    };
}
