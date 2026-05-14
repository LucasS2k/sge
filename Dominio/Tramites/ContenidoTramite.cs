public record class ContenidoTramite
{
    public string Valor {get;}
    public ContenidoTramite(string valor)
    {
        if(string.IsNullOrEmpty(valor))
            throw new DomainException("El contenido del tramite no puede estar vacío");
        Valor = valor;
    }
    //parse para convertir de string a ContenidoTramite
    public static ContenidoTramite Parse(string valor)
    {
        return new ContenidoTramite(valor);
    }
}