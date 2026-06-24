namespace Dominio.Comun;
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}