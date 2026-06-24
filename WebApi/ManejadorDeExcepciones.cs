namespace WebApi;
using Aplicacion.Comun;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dominio.Comun;

public class ManejadorDeExcepciones : IExceptionHandler
{   //maneja las excepciones de la aplicacion y devuelve un status code adecuado
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, titulo) = exception switch
        {
            DomainException        => (StatusCodes.Status400BadRequest,           "Error de negocio"),
            AuthorizationException => (StatusCodes.Status403Forbidden,            "Acceso denegado"),
            NotFoundException      => (StatusCodes.Status404NotFound,             "No encontrado"),
            _                      => (StatusCodes.Status500InternalServerError,  "Error interno")
        };

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title  = titulo,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}