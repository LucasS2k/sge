namespace Aplicacion.Autorizacion;
using Dominio.Autorizacion;
using Aplicacion.Usuarios; 

public class AutorizacionService : IAutorizacionService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AutorizacionService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    //la excepcion se maneja en el caso de uso
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        var usuario = _usuarioRepository.ObtenerUsuarioPorId(idUsuario);
        
        if (usuario == null) return false;
        if (usuario.EsAdministrador) return true; //tiene todos los permisos
        if (permiso == Permiso.TramiteBaja && usuario.Permisos.Contains(Permiso.ExpedienteBaja))//uno implica otro necesariamente
    {
        return true;
    }
        return usuario.TienePermiso(permiso);
    }
}