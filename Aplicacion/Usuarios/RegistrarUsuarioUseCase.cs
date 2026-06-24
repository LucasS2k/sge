namespace Aplicacion.Usuarios;
using Aplicacion.Autorizacion;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;
using Dominio.Comun;
public class RegistrarUsuarioUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo _unidadDeTrabajo)
{
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        var existente = usuarioRepo.ObtenerUsuarioPorCorreo(request.CorreoElectronico);
        if (existente is not null)
            throw new DomainException("Ese correo ya esta en uso");
 
        var hash = Hash.Calcular(request.Contrasena);
        var usuario = new Usuario(request.Nombre, request.CorreoElectronico, hash);
 
        usuarioRepo.AgregarUsuario(usuario);
        _unidadDeTrabajo.Guardar();
 
        return new RegistrarUsuarioResponse(usuario.Id, usuario.Nombre, usuario.CorreoElectronico);
    }
}