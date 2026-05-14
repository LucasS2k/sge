namespace Infraestructura.Autorizacion;
using Dominio.Autorizacion;
public class AuthorizationService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        //provisorio, todos tienen todos los permisos
        return true;
    }
}