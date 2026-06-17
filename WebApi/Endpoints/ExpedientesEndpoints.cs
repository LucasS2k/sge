namespace WebApi.Endpoints;
using Aplicacion.Comun;
using Aplicacion.Expedientes;
using System.Security.Claims;
using Dominio.Autorizacion;
public static class ExpedientesEndpoints
{
    public static void MapExpedientesEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/expedientes")
                       .WithTags("Expedientes")
                       .RequireAuthorization();

        grupo.MapGet("/", (ListarExpedientesUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new ListarExpedientesRequest(ObtenerUsuarioId(user)));
            return Results.Ok(response);
        }).WithSummary("Listar todos los expedientes");

        grupo.MapPost("/", (AltaExpedienteRequest request, AltaExpedienteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(request with { IdUsuario = ObtenerUsuarioId(user) });
            return Results.Created($"/expedientes/{response.Id}", response);
        }).WithSummary("Crear un nuevo expediente");

        grupo.MapPut("/{id:guid}/caratula", (Guid id, ModificarCaratulaExpedienteRequest request, ModificarCaratulaExpedienteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(request with { IdExpediente = id, IdUsuario = ObtenerUsuarioId(user) });
            return Results.Ok(response);
        }).WithSummary("Modificar la carátula de un expediente");


        grupo.MapDelete("/{id:guid}", (Guid id, BajaExpedienteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new BajaExpedienteRequest(id, ObtenerUsuarioId(user)));
            return Results.Ok(response);
        }).WithSummary("Eliminar un expediente y sus trámites (baja en cascada)");
    }

    private static Guid ObtenerUsuarioId(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst("ID")?.Value
            ?? throw new AuthorizationException("Token invalido");
        return Guid.Parse(userIdString);
    }
}