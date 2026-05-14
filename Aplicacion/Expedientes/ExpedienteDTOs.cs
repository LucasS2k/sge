namespace Aplicacion.Expedientes;

//requests
public record AltaExpedienteRequest(string Caratula, Guid IdUsuario);

public record BajaExpedienteRequest(Guid IdExpediente, Guid IdUsuario);

public record ModificarExpedienteRequest(Guid IdExpediente, string NuevaCaratula, Guid IdUsuario);

public record ListarExpedientesRequest(Guid IdUsuario);

//responses
public record ExpedienteResponse(
    Guid Id,
    string Caratula, 
    string Estado, 
    DateTime FechaCreacion, 
    Guid IdUsuario
    );
public record AltaExpedienteResponse(
    Guid Id, 
    string Caratula, 
    DateTime FechaCreacion
);
public record BajaExpedienteResponse(Guid Id, bool Exito);

public record ListarExpedientesResponse(IEnumerable<ExpedienteResponse> Expedientes);

public record ModificarExpedienteResponse(Guid Id, bool Exito);

public record ModificarCaratulaExpedienteResponse(Guid Id, bool Exito);
