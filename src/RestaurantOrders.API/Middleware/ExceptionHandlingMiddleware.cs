namespace RestaurantOrders.API.Middleware;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrders.Domain.Exceptions;

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
            logger.LogError(ex, "Exceção não tratada: {Message}", ex.Message);
            await HandleAsync(context, ex);
        }
    }

    private static async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (status, title, errors) = exception switch
        {
            ValidationException ve => (
                StatusCodes.Status400BadRequest,
                "Erro de validação.",
                ve.Errors.Select(e => e.ErrorMessage).ToArray()),

            NotFoundException => (
                StatusCodes.Status404NotFound,
                exception.Message,
                Array.Empty<string>()),

            DomainException => (
                StatusCodes.Status422UnprocessableEntity,
                exception.Message,
                Array.Empty<string>()),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Não autorizado.",
                Array.Empty<string>()),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro inesperado.",
                Array.Empty<string>())
        };

        context.Response.StatusCode      = status;
        context.Response.ContentType     = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = status,
            Title  = title,
            Extensions = { ["errors"] = errors.Length > 0 ? errors : null }
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
