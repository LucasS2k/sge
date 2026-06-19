namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;


public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;

    public ModificarMisDatosUseCase(IUsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }
    //un usuario solo puede modficar sus propios datos, no se requiere autorización adicional
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request)
    {
        var usuario = _usuarioRepo.ObtenerUsuarioPorId(request.IdUsuarioAutenticado)
            ?? throw new NotFoundException("No existe el usuario solicitado.");

        usuario.ModificarDatos(request.NuevoNombre, request.NuevoCorreo); //POSIBLE NULL REVISAR

        if (!string.IsNullOrWhiteSpace(request.NuevaContrasena))
            usuario.CambiarContrasena(Hash.Calcular(request.NuevaContrasena));

        _usuarioRepo.ModificarUsuario(usuario);

        return new ModificarMisDatosResponse(usuario.Id, usuario.Nombre, usuario.CorreoElectronico);
    }
}