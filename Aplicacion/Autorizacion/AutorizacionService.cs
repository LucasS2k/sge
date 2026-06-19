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

        return usuario.TienePermiso(permiso);
    }
}