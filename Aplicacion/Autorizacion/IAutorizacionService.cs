using Dominio.Autorizacion;
public interface IAutorizacionService
{
    bool VerificarPermiso(Guid idUsuario, Permiso permiso)
 {
    //provisorio, todos tienen todos los permisos
    return true;
 }
 }
