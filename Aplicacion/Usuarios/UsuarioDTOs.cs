namespace Aplicacion.Usuarios;
using Dominio.Autorizacion;
using System.Text.Json.Serialization;

// Requests
public record RegistrarUsuarioRequest(string Nombre, string CorreoElectronico, string Contrasena);
public record LoginRequest(string CorreoElectronico, string Contrasena);
public record ListarUsuariosRequest(Guid IdAdministrador);
public record BajaUsuarioRequest(Guid IdAdministrador, Guid IdUsuarioAEliminar);
public record ModificarPermisosUsuarioRequest(Guid IdAdministrador, Guid IdUsuario, IEnumerable<Permiso> NuevosPermisos);
public record ModificarPermisosBody(List<string> Permisos);

public record ModificarMisDatosRequest(string? NuevoNombre, string? NuevoCorreo, string? NuevaContrasena)
{
    [JsonIgnore] public Guid IdUsuarioAutenticado { get; init; }= Guid.Empty; //para que no lo pida en el request, el usuario sigue identificandose con el token
}
// Responses
public record RegistrarUsuarioResponse(Guid Id, string Nombre, string CorreoElectronico);
public record LoginResponse(string Token);
public record UsuarioResponse(Guid Id, string Nombre, string CorreoElectronico, bool EsAdministrador, IEnumerable<string> Permisos);
public record ListarUsuariosResponse(IEnumerable<UsuarioResponse> Usuarios);
public record BajaUsuarioResponse(Guid Id, bool Exito);
public record ModificarPermisosUsuarioResponse(Guid Id, bool Exito);

public record ModificarMisDatosResponse(Guid Id, string Nombre, string CorreoElectronico);