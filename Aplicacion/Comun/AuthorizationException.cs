namespace Aplicacion.Comun;
public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message)
    {   //mensaje general, a veces es redundante
        "No posee permisos para realizar esta accion: ".ToString();
    }
}