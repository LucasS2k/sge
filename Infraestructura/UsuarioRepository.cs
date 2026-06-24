using Aplicacion.Usuarios;
using Dominio.Usuarios;
using Microsoft.EntityFrameworkCore;
using Dominio.Autorizacion;
namespace Infraestructura;
//interaccion con la base de datos para la entidad Usuario
public class UsuarioRepository : IUsuarioRepository
{
    private readonly SgeContext _context;

    public UsuarioRepository(SgeContext context)
    {
        _context = context;
    }

    public void AgregarUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
    }

    public void ModificarUsuario(Usuario usuario)
{
    var tracked = _context.Usuarios
        .FirstOrDefault(u => u.Id == usuario.Id);
    if (tracked != null)
    {
        tracked.Permisos = new List<Permiso>(usuario.Permisos);
    }
}

    public void EliminarUsuario(Guid id)
    {
        var usuario = _context.Usuarios.Find(id) 
            ?? throw new Exception("Usuario no encontrado");
        _context.Usuarios.Remove(usuario);
    }
//
    public Usuario? ObtenerUsuarioPorId(Guid id)
{
    return _context.Usuarios
        .FirstOrDefault(u => u.Id == id);
}
    //para el login
    public Usuario? ObtenerUsuarioPorCorreo(string correoElectronico)
    {
        return _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == correoElectronico);
    }

    public IEnumerable<Usuario> ObtenerTodosLosUsuarios()
    {
        return _context.Usuarios.ToList();
    }
}