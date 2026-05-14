public record class Caratula
{
    public string Valor {get;}
    public Caratula(string valor)
    {
        if(string.IsNullOrEmpty(valor))
            throw new DomainException("La caratula no puede ser nula o estar vacía");
        Valor = valor;
    }
    public static Caratula Parse(string valor)
    {
        return new Caratula(valor);
    }
}