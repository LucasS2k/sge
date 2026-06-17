namespace WebApi.Endpoints;
using Aplicacion.Comun;
using Aplicacion.Tramites;
using System.Security.Claims;

public static class TramitesEndpoints
{
    public static void MapTramitesEndpoints(this IEndpointRouteBuilder app)
    {

        var grupo = app.MapGroup("/tramites").WithTags("Tramites").RequireAuthorization();

        grupo.MapGet("/", (ListarTramitesUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new ListarTramitesRequest(ObtenerUsuarioId(user)));
            return Results.Ok(response);
        }).WithSummary("Listar todos los tramites");


        grupo.MapPost("/", (AltaTramiteRequest request, AltaTramiteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(request with { IdUsuario = ObtenerUsuarioId(user) });
            return Results.Created($"/tramites/{response.Id}", response);
        }).WithSummary("Crear un nuevo tramite");

        grupo.MapDelete("/{id:guid}", (Guid id, BajaTramiteUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new BajaTramiteRequest(id, ObtenerUsuarioId(user)));
            return Results.Ok(response);
        }).WithSummary("Eliminar un tramite");
    }
    private static Guid ObtenerUsuarioId(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst("ID")?.Value
            ?? throw new AuthorizationException("Token invalido");
        return Guid.Parse(userIdString);
    }
}