namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;
public class ListarUsuariosUseCase(IUsuarioRepository usuarioRepo)
{
    public ListarUsuariosResponse Ejecutar(ListarUsuariosRequest request)
    {   //solo un admin puede listar usuarios, no se requiere un permiso especifico para listar usuarios
        var admin = usuarioRepo.ObtenerUsuarioPorId(request.IdAdministrador)
            ?? throw new NotFoundException("Usuario no encontrado");
 
        if (!admin.EsAdministrador)
            throw new AuthorizationException("No es administrador, no puede realizar esta accion");
 
        var usuarios = usuarioRepo.ObtenerTodosLosUsuarios()
            .Select(u => new UsuarioResponse(
                u.Id, u.Nombre, u.CorreoElectronico, u.EsAdministrador,
                u.Permisos.Select(p => p.ToString())));
 
        return new ListarUsuariosResponse(usuarios);
    }
}