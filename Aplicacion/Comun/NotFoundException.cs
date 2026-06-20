namespace Aplicacion.Comun;
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {   //mensaje general, a veces es redundante
        "No encontrado: ".ToString();
    }
}