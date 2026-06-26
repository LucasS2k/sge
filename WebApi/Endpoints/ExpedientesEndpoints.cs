namespace WebApi.Endpoints;
using Aplicacion.Comun;
using Aplicacion.Expedientes;
using Aplicacion.Tramites;
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
        //falta testear
        grupo.MapGet("/{id:guid}/tramites", (Guid id, ListarTramitesPorExpedienteUseCase useCase) =>
        {
             var response = useCase.Ejecutar(id);
             return Results.Ok(response);
        }).WithSummary("Listar todos los tramites de un expediente");
        //falta testear
        grupo.MapGet("/{id:guid}/detallado", (Guid id, ObtenerConTramitesUseCase useCase) =>
        {
            var response = useCase.Ejecutar(new ObtenerExpedienteRequest(id, Guid.Empty));
            return Results.Ok(response);
        }).WithSummary("Obtener expediente con tramites detallados");

        grupo.MapPost("/", (AltaExpedienteRequest request, AltaExpedienteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(request with { IdUsuario = ObtenerUsuarioId(user) });
            return Results.Created($"/expedientes/{response.Id}", response);
        }).WithSummary("Crear un nuevo expediente");

        grupo.MapPut("/{id:guid}/caratula", (Guid id, ModificarCaratulaExpedienteRequest request, ModificarCaratulaExpedienteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(request with { IdExpediente = id, IdUsuario = ObtenerUsuarioId(user) });
            return Results.Ok(response);
        }).WithSummary("Modificar la caratula de un expediente");


        grupo.MapDelete("/{id:guid}", (Guid id, BajaExpedienteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new BajaExpedienteRequest(id, ObtenerUsuarioId(user)));
            return Results.Ok(response);
        }).WithSummary("Eliminar un expediente y sus tramites");
    }

    private static Guid ObtenerUsuarioId(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst("ID")?.Value
            ?? throw new AuthorizationException("Token invalido");
        return Guid.Parse(userIdString);
    }
}