namespace Aplicacion.Expedientes;

//requests
public record AltaExpedienteRequest(string Caratula, Guid IdUsuario);

public record BajaExpedienteRequest(Guid IdExpediente, Guid IdUsuario);

public record ModificarExpedienteRequest(Guid IdExpediente, string NuevaCaratula, Guid IdUsuario);


//responses
public record ExpedienteResponse(
    Guid Id,
    string Caratula, 
    string Estado, 
    DateTime FechaCreacion, 
    Guid IdUsuario
    );
    