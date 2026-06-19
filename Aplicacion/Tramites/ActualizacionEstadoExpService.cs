namespace Aplicacion.Tramites;
using Aplicacion.Expedientes;
using Aplicacion.Comun;
public class ActualizacionEstadoExpService
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly ITramiteRepository _tramiteRepo;
    public ActualizacionEstadoExpService(IExpedienteRepository expedienteRepo, ITramiteRepository tramiteRepo)
    {
        _expedienteRepo = expedienteRepo;
        _tramiteRepo = tramiteRepo;
    }

    public ActualizacionEstadoExpResponse Ejecutar(Guid expedienteId, Guid idUsuario)
    {
        var expediente = _expedienteRepo.ObtenerExpedientePorId(expedienteId) 
            ?? throw new NotFoundException("No existe el expediente solicitado");
        var tramites = _tramiteRepo.ObtenerTodosLosTramites();

        var ultimoTramite = tramites.OrderByDescending(t => t.FechaCreacion).FirstOrDefault();
        var estado = expediente.Estado.ToString();
        bool huboCambio = expediente.ActualizarEstado(ultimoTramite?.Etiqueta, idUsuario);

        if (huboCambio)
        {
            _expedienteRepo.ModificarExpediente(expediente);
        }
        return new ActualizacionEstadoExpResponse(expedienteId, estado, huboCambio);
    }
}