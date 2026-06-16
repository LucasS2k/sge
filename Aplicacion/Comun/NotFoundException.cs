namespace Aplicacion.Comun;
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
        "No encontrado: ".ToString();
    }
}