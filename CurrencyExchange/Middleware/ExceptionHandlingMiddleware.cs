using CurrencyExchange.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var (status, title) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflicto"),
                BadHttpRequestException => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
                OperationCanceledException => (StatusCodes.Status499ClientClosedRequest, "Solicitud cancelada"),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
            };

            if (status == StatusCodes.Status500InternalServerError)
                logger.LogError(ex, "Excepción no controlada");
            else
                logger.LogWarning("{Title}: {Message}", title, ex.Message);

            if (context.Response.HasStarted) throw;

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = ex switch
                {
                    BadHttpRequestException => "El cuerpo de la solicitud no es válido.",
                    _ when status == StatusCodes.Status500InternalServerError => "Ocurrió un error inesperado.",
                    _ => ex.Message
                },
                Instance = context.Request.Path
            });
        }
    }
}
