namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;

public class LoginUseCase(IUsuarioRepository usuarioRepo, ITokenProvider tokenProvider)
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        var usuario = usuarioRepo.ObtenerUsuarioPorCorreo(request.CorreoElectronico)
            ?? throw new AuthorizationException("Credenciales invalidas");
 
        if (!Hash.Verificar(request.Contrasena, usuario.ContrasenaHash))
            throw new AuthorizationException("Credenciales invalidas");
 
        var token = tokenProvider.GenerarToken(usuario);
        return new LoginResponse(token);
    }
}