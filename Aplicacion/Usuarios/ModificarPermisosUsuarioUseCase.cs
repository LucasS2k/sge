namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;
public class ModificarPermisosUsuarioUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo uow)
{
    public ModificarPermisosUsuarioResponse Ejecutar(ModificarPermisosUsuarioRequest request)
    {
        var admin = usuarioRepo.ObtenerUsuarioPorId(request.IdAdministrador)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        if (!admin.EsAdministrador)
            throw new AuthorizationException("No es admin/No posee permisos para modificar permisos de usuarios");
 
        var usuario = usuarioRepo.ObtenerUsuarioPorId(request.IdUsuario)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        usuario.ReemplazarPermisos(request.NuevosPermisos);
        usuarioRepo.ModificarUsuario(usuario);
        uow.Guardar();
 
        return new ModificarPermisosUsuarioResponse(usuario.Id, true);
    }
}