namespace Aplicacion.Comun;
public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message)
    {
        "No posee permisos para realizar esta accion: ".ToString();
    }
}