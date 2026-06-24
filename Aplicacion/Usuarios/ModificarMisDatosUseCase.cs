namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;


public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarMisDatosUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepo = usuarioRepo;
         _unidadDeTrabajo =  unidadDeTrabajo;
    }
    //un usuario solo puede modficar sus propios datos, no se requiere autorización adicional
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request)
{ 
        var usuario = _usuarioRepo.ObtenerUsuarioPorId(request.IdUsuarioAutenticado)
            ?? throw new NotFoundException("No existe el usuario solicitado");
Console.WriteLine($"Usuario encontrado: {usuario.Nombre}");
        if (!string.IsNullOrWhiteSpace(request.NuevoNombre))
            usuario.CambiarNombre(request.NuevoNombre);

        if (!string.IsNullOrWhiteSpace(request.NuevoCorreo))
            usuario.CambiarCorreo(request.NuevoCorreo);

        if (!string.IsNullOrWhiteSpace(request.NuevaContrasena))
            usuario.CambiarContrasena(Hash.Calcular(request.NuevaContrasena));
        _usuarioRepo.ModificarUsuario(usuario);
        _unidadDeTrabajo.Guardar();
        return new ModificarMisDatosResponse(usuario.Id, usuario.Nombre, usuario.CorreoElectronico);
    }
}