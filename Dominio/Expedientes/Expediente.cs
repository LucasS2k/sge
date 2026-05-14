
using Dominio.Tramites;
public class Expediente
{
    public Guid Id{get; private set;}
    public Guid ExpedienteId{ get; private set; }
    public Caratula Caratula{ get; private set; }
    public DateTime FechaCreacion{ get; init; }
    public DateTime FechaUltimaModificacion{ get; private set; }
    public Guid UsuarioUltimoCambio{ get; private set; }
    public EstadoExpediente Estado{ get; private set; }
//constructor base
    public Expediente( Caratula caratula, Guid usuarioUltimoCambio)
    {
        if (usuarioUltimoCambio == Guid.Empty)
            throw new DomainException("Usuario requerido");
        Id = Guid.NewGuid();
        ExpedienteId = Id;
        Caratula = caratula;
        FechaCreacion = DateTime.UtcNow;
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        Estado = EstadoExpediente.RecienIniciado;
    }
//constructor de 7 argumentos para reconstrucción
    public Expediente(Guid id, Guid expedienteId, Caratula caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, EstadoExpediente estado)
    {
        Id = id;
        ExpedienteId = expedienteId;
        Caratula = caratula;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUltimaModificacion;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        Estado = estado;
    }

    public void ModificarCaratula(Caratula nuevaCaratula, Guid IdUsuario)
    {
        if (IdUsuario == Guid.Empty)
            throw new DomainException("Usuario requerido");
        Caratula = nuevaCaratula;
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = IdUsuario;
    }
    public bool ActualizarEstado(EstadoTramite? ultimaEtiqueta, Guid IdUsuario)
    {
        if (IdUsuario == Guid.Empty)
            throw new DomainException("Usuario requerido");
        EstadoExpediente estadoPrevio = this.Estado;
        this.Estado = ultimaEtiqueta switch
        {
            EstadoTramite.PaseAEstudio => EstadoExpediente.ParaResolver,
            EstadoTramite.Resolucion => EstadoExpediente.ConResolucion,
            EstadoTramite.PaseAlArchivo => EstadoExpediente.Finalizado,
            null => EstadoExpediente.RecienIniciado,
            _ => EstadoExpediente.RecienIniciado
        };
        if (this.Estado == estadoPrevio)
            return false;
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = IdUsuario;
        return true;
    }
    public void CambiarEstado(EstadoExpediente nuevoEstado, Guid IdUsuario)
    {
        if (IdUsuario == Guid.Empty)
            throw new DomainException("Usuario requerido");
        Estado = nuevoEstado;
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = IdUsuario;
    }

}