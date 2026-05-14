namespace Aplicacion.Tramites;
using Dominio.Tramites;
//requests
public record AltaTramiteRequest(Guid IdExpediente, string Etiqueta, ContenidoTramite Contenido, Guid IdUsuario);
public record ModificarTramiteRequest(Guid IdTramite, string Etiqueta, ContenidoTramite Contenido, Guid IdUsuario);
public record BajaTramiteRequest(Guid IdTramite, Guid IdUsuario);

public record ListarTramitesRequest(Guid IdUsuario);

//responses
public record TramiteResponse(
    Guid Id,
    Guid IdExpediente,
    string Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    Guid IdUsuario
);

public record AltaTramiteResponse(
    Guid Id,
    Guid IdExpediente,
    string Etiqueta,
    string Contenido,
    DateTime FechaCreacion
);
public record BajaTramiteResponse(Guid Id, bool Exito);
public record ListarTramitesResponse(IEnumerable<TramiteResponse> Tramites);
public record ActualizacionEstadoExpResponse(Guid IdExpediente, string NuevoEstado, bool Cambio);
