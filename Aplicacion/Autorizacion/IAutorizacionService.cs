using Dominio.Autorizacion;
public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
 }
