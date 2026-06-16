using Dominio.Usuarios;

namespace Aplicacion.Usuarios;

public interface IUsuarioRepository
{
    void AgregarUsuario(Usuario usuario);
    void ModificarUsuario(Usuario usuario);
    void EliminarUsuario(Guid id);
    Usuario? ObtenerUsuarioPorId(Guid id);
    Usuario? ObtenerUsuarioPorCorreo(string correoElectronico);
    IEnumerable<Usuario> ObtenerTodosLosUsuarios();
}