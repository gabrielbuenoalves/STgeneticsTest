using GoodHamburger.Application.Exceptions;
using GoodHamburger.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace GoodHamburger.Web.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Erro nao tratado durante a requisicao.");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            DomainException => (HttpStatusCode.BadRequest, "Erro de validacao de negocio"),
            ResourceNotFoundException => (HttpStatusCode.NotFound, "Recurso nao encontrado"),
            _ => (HttpStatusCode.InternalServerError, "Erro interno")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = JsonSerializer.Serialize(new
        {
            title,
            status = context.Response.StatusCode,
            detail = exception.Message
        });

        return context.Response.WriteAsync(payload);
    }
}