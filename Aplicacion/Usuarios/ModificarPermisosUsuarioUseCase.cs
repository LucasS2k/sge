namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;
public class ModificarPermisosUsuarioUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo _unidadDeTrabajo)
{
    public ModificarPermisosUsuarioResponse Ejecutar(ModificarPermisosUsuarioRequest request)
    {   //solo un admin puede modificar permisos de usuarios, no se requiere un permiso especifico para modificar permisos de usuarios
        var admin = usuarioRepo.ObtenerUsuarioPorId(request.IdAdministrador)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        if (!admin.EsAdministrador)
            throw new AuthorizationException("No es administrador, no puede realizar esta accion");
 
        var usuario = usuarioRepo.ObtenerUsuarioPorId(request.IdUsuario)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        usuario.ReemplazarPermisos(request.NuevosPermisos);
        Console.WriteLine($"Nuevos permisos del request: {string.Join(", ", request.NuevosPermisos)}");
        usuarioRepo.ModificarUsuario(usuario);
        _unidadDeTrabajo.Guardar();
 
        return new ModificarPermisosUsuarioResponse(usuario.Id, true);
    }
}