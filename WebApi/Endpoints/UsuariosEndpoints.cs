namespace WebApi.Endpoints;
using Aplicacion.Comun;
using Aplicacion.Usuarios;
using Dominio.Autorizacion;
using System.Security.Claims;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/usuarios").WithTags("Usuarios");

        grupo.MapPost("/registrar", (RegistrarUsuarioRequest request, RegistrarUsuarioUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Created($"/usuarios/{response.Id}", response);
        }).WithSummary("Registrar un nuevo usuario");

        grupo.MapPost("/login", (LoginRequest request, LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).WithSummary("Iniciar sesion y obtener token");

        grupo.MapPut("/mis-datos", (ModificarMisDatosRequest request, ModificarMisDatosUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(request with { IdUsuarioAutenticado = ObtenerUsuarioId(user) });
            return Results.Ok(response);
        }).RequireAuthorization().WithSummary("Modificar mis datos o contraseña");

        grupo.MapGet("/", (ListarUsuariosUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new ListarUsuariosRequest(ObtenerUsuarioId(user)));
            return Results.Ok(response);
        }).RequireAuthorization().WithSummary("Listar todos los usuarios");

        grupo.MapDelete("/{id:guid}", (Guid id, BajaUsuarioUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new BajaUsuarioRequest(ObtenerUsuarioId(user), id));
            return Results.Ok(response);
        }).RequireAuthorization().WithSummary("Eliminar un usuario");

        grupo.MapPut("/{id:guid}/permisos", (Guid id, IEnumerable<Permiso> body, ModificarPermisosUsuarioUseCase useCase, ClaimsPrincipal user) =>
        {
            var response = useCase.Ejecutar(new ModificarPermisosUsuarioRequest(ObtenerUsuarioId(user), id, body));
            return Results.Ok(response);
        }).RequireAuthorization().WithSummary("Modificar permisos de un usuario");
    }

    private static Guid ObtenerUsuarioId(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst("ID")?.Value
            ?? throw new AuthorizationException("Token invalido");
        return Guid.Parse(userIdString);
    }
}