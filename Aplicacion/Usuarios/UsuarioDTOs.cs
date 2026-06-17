namespace Aplicacion.Usuarios;
using Dominio.Autorizacion;

// Requests
public record RegistrarUsuarioRequest(string Nombre, string CorreoElectronico, string Contrasena);
public record LoginRequest(string CorreoElectronico, string Contrasena);
public record ListarUsuariosRequest(Guid IdAdministrador);
public record BajaUsuarioRequest(Guid IdAdministrador, Guid IdUsuarioAEliminar);
public record ModificarPermisosUsuarioRequest(Guid IdAdministrador, Guid IdUsuario, IEnumerable<Permiso> NuevosPermisos);

public record ModificarMisDatosRequest(Guid IdUsuarioAutenticado,string?NuevoNombre, string? NuevoCorreo, string? NuevaContrasena);
// Responses
public record RegistrarUsuarioResponse(Guid Id, string Nombre, string CorreoElectronico);
public record LoginResponse(string Token);
public record UsuarioResponse(Guid Id, string Nombre, string CorreoElectronico, bool EsAdministrador, IEnumerable<string> Permisos);
public record ListarUsuariosResponse(IEnumerable<UsuarioResponse> Usuarios);
public record BajaUsuarioResponse(Guid Id, bool Exito);
public record ModificarPermisosUsuarioResponse(Guid Id, bool Exito);

public record ModificarMisDatosResponse(Guid Id, string Nombre, string CorreoElectronico);