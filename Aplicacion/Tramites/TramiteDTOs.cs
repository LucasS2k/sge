namespace Aplicacion.Tramites;
using Dominio.Tramites;
//requests
public record AltaTramiteRequest(Guid IdExpediente, string Etiqueta, ContenidoTramite Contenido, Guid IdUsuario);
public record ModificarTramiteRequest(Guid IdTramite, string Etiqueta, ContenidoTramite Contenido, Guid IdUsuario);
public record BajaTramiteRequest(Guid IdTramite, Guid IdUsuario);


//responses
public record TramiteResponse(
    Guid Id,
    Guid IdExpediente,
    string Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    Guid IdUsuario
);
