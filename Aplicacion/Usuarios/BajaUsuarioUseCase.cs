namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Aplicacion.Unidad;
public class BajaUsuarioUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo uow)
{
    public BajaUsuarioResponse Ejecutar(BajaUsuarioRequest request)
    {
        var admin = usuarioRepo.ObtenerUsuarioPorId(request.IdAdministrador)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        if (!admin.TienePermiso(Permiso.UsuarioBaja))
            throw new AuthorizationException("No posee permisos para eliminar usuarios");
 
        var usuarioAEliminar = usuarioRepo.ObtenerUsuarioPorId(request.IdUsuarioAEliminar)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        usuarioRepo.EliminarUsuario(usuarioAEliminar.Id);
        uow.Guardar();
 
        return new BajaUsuarioResponse(usuarioAEliminar.Id, true);
    }
}