namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Aplicacion.Unidad;
public class BajaUsuarioUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo _unidadDeTrabajo)
{
    public BajaUsuarioResponse Ejecutar(BajaUsuarioRequest request)
    {
        var admin = usuarioRepo.ObtenerUsuarioPorId(request.IdAdministrador)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        if (!admin.EsAdministrador)
            throw new AuthorizationException("No es admin/No posee permisos para eliminar usuarios");
 
        var usuarioAEliminar = usuarioRepo.ObtenerUsuarioPorId(request.IdUsuarioAEliminar)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        usuarioRepo.EliminarUsuario(usuarioAEliminar.Id);
        _unidadDeTrabajo.Guardar();
 
        return new BajaUsuarioResponse(usuarioAEliminar.Id, true);
    }
}