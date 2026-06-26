using Dominio.Tramites;
using Dominio.Comun;
public class Tramite
{
    public Guid Id{get; private set;}
    public Guid ExpedienteId{ get; private set; }
    public EstadoTramite? Etiqueta{ get; private set; }
    public ContenidoTramite Contenido{ get; private set; } = null!;
    public DateTime FechaCreacion{ get; init; }
    public DateTime FechaUltimaModificacion{ get; private set; }
    public Guid UsuarioUltimoCambio{ get; private set; }

    public Expediente Expediente { get; private set; } = null!;
    //constructor para crear un nuevo tramite
    public Tramite(Guid expedienteId, EstadoTramite? etiqueta, ContenidoTramite contenido, Guid usuarioUltimoCambio)
    {
        if (usuarioUltimoCambio == Guid.Empty)
            throw new DomainException("Usuario requerido");
        if (FechaUltimaModificacion < FechaCreacion)
            throw new DomainException("La fecha de ultima modificación no puede ser anterior a la fecha de creacion");
        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        FechaCreacion = DateTime.UtcNow;
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = usuarioUltimoCambio;
    }
    //reconstructor de 6 parametros para cuando leemos en infraestructura
    public Tramite(Guid id, Guid expedienteId, EstadoTramite etiqueta, ContenidoTramite contenido, DateTime fecha, Guid usuarioId)
    {
        Id = id;
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        FechaCreacion = fecha;
        UsuarioUltimoCambio = usuarioId;
    }

    protected Tramite() { }
 
    public void ModificarContenido(EstadoTramite nuevaEtiqueta, ContenidoTramite nuevoContenido, Guid idUsuario)
    {
        if (idUsuario == Guid.Empty)
            throw new DomainException("Usuario requerido");
 
        Etiqueta = nuevaEtiqueta;
        Contenido = nuevoContenido;
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = idUsuario;
    }
}