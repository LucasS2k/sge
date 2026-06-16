using Dominio.Autorizacion;

namespace Dominio.Usuarios;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string CorreoElectronico { get; private set; }
    public string ContrasenaHash { get; private set; }
    public bool EsAdministrador { get; private set; }

    private readonly List<Permiso> _permisos;
    public IReadOnlyCollection<Permiso> Permisos => _permisos.AsReadOnly();

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
        _permisos = new List<Permiso>();
    }
    public Usuario(Guid id, string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador, IEnumerable<Permiso> permisos)
    {
        Id = id;
        Nombre = nombre;
        CorreoElectronico = correoElectronico;
        ContrasenaHash = contrasenaHash;
        EsAdministrador = esAdministrador;
        _permisos = permisos.ToList();
    }
    public void ModificarDatos(string nuevoNombre, string nuevoCorreo)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new DomainException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(nuevoCorreo))
            throw new DomainException("El correo electrónico es obligatorio.");

        Nombre = nuevoNombre;
        CorreoElectronico = nuevoCorreo.ToLowerInvariant();
    }

    public void CambiarContrasena(string nuevoHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoHash))
            throw new DomainException("La contraseña es obligatoria.");

        ContrasenaHash = nuevoHash;
    }
    public void AgregarPermiso(Permiso permiso)
    {
        if (!_permisos.Contains(permiso))
            _permisos.Add(permiso);
    }

    public void RemoverPermiso(Permiso permiso)
    {
        _permisos.Remove(permiso);
    }

    public void ReemplazarPermisos(IEnumerable<Permiso> nuevosPermisos)
    {
        _permisos.Clear();
        _permisos.AddRange(nuevosPermisos.Distinct());
    }

    public bool TienePermiso(Permiso permiso)
    {
        if (permiso == Permiso.TramiteBaja && _permisos.Contains(Permiso.ExpedienteBaja))
            return true;

        return _permisos.Contains(permiso);
    }
}