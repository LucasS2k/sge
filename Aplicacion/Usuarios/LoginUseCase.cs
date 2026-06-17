namespace Aplicacion.Usuarios;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Aplicacion.Unidad;

public class LoginUseCase(IUsuarioRepository usuarioRepo, IJwtService jwtService)
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        var usuario = usuarioRepo.ObtenerUsuarioPorCorreo(request.CorreoElectronico)
            ?? throw new AuthorizationException("Credenciales invalidas");
 
        if (!HashService.Verificar(request.Contrasena, usuario.ContrasenaHash))
            throw new AuthorizationException("Credenciales invalidas");
 
        var token = jwtService.GenerarToken(usuario.Id);
        return new LoginResponse(token);
    }
}