using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_WebAPI.Utils;

// A estrutura de como ProblemExeption será utilizando e seus parâmetros: 
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
        // Verifica se a exceção é do tipo ProblemExeption; do contrário este handler não a trata
        if (exception is not ProblemExeption problemExeption)
        {
            return true;
        }

        // Parâmetros adicionados
        var problemDetails = new ProblemDetails
        {
            Status = problemExeption.StatusCode,
            Title = problemExeption.Error,
            Detail = problemExeption.Message,
            Type = GetProblemType(problemExeption.StatusCode)
        };

        // Valores atribuidos para ProblemDetails a partir da exceção, padronizando a resposta de erro
        httpContext.Response.StatusCode = problemExeption.StatusCode;
        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            }
        );
    }

    // Lista dos possíveis erros retornados e suas mensagens customizadas
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
