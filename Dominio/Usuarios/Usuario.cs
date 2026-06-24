using Dominio.Autorizacion;
using Dominio.Comun;
namespace Dominio.Usuarios;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string CorreoElectronico { get; private set; }
    public string ContrasenaHash { get; private set; }
    public bool EsAdministrador { get; private set; }

    public List<Permiso> Permisos {get; set;} =new();

    public Usuario(string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador = false)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio");
        if (string.IsNullOrWhiteSpace(correoElectronico))
            throw new DomainException("El correo electrónico es obligatorio");
        if (string.IsNullOrWhiteSpace(contrasenaHash))
            throw new DomainException("La contraseña es obligatoria");

        Id = Guid.NewGuid();
        Nombre = nombre;
        CorreoElectronico = correoElectronico.ToLowerInvariant();
        ContrasenaHash = contrasenaHash;
        EsAdministrador = esAdministrador;
        Permisos = new List<Permiso>();
    }
    public Usuario(Guid id, string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador, IEnumerable<Permiso> permisos)
    {
        Id = id;
        Nombre = nombre;
        CorreoElectronico = correoElectronico;
        ContrasenaHash = contrasenaHash;
        EsAdministrador = esAdministrador;
        Permisos = permisos.ToList();
    }

    public void CambiarNombre(string nuevoNombre)
    {
     if (string.IsNullOrWhiteSpace(nuevoNombre))
         throw new DomainException("El nombre es obligatorio");
      Nombre = nuevoNombre;
    }

    public void CambiarCorreo(string nuevoCorreo)
    {
     if (string.IsNullOrWhiteSpace(nuevoCorreo))
         throw new DomainException("El correo electronico es obligatorio");
     CorreoElectronico = nuevoCorreo.ToLowerInvariant();
    }

    public void CambiarContrasena(string nuevoHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoHash))
            throw new DomainException("La contraseña es obligatoria");

        ContrasenaHash = nuevoHash;
    }
    public void AgregarPermiso(Permiso permiso)
    {
        if (!Permisos.Contains(permiso))
            Permisos.Add(permiso);
    }

    public void RemoverPermiso(Permiso permiso)
    {
        Permisos.Remove(permiso);
    }

    public void ReemplazarPermisos(IEnumerable<Permiso> nuevosPermisos)
    {
        Permisos.Clear();
        Permisos.AddRange(nuevosPermisos.Distinct());
    }

    public bool TienePermiso(Permiso permiso)
    {
        if (Permisos.Contains(permiso)) return true;
        else if (EsAdministrador) return true; 
        else return false;
    }
}