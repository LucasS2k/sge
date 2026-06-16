namespace Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Aplicacion.Comun;
using Dominio.Autorizacion;

public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarExpedienteUseCase(
        IExpedienteRepository expedienteRepo,
        ITramiteRepository tramiteRepo,
        IAutorizacionService autorizacionService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepo = expedienteRepo;
        _tramiteRepo = tramiteRepo;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public BajaExpedienteResponse Ejecutar(BajaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
            throw new AuthorizationException("No posee permiso para dar de baja expedientes.");

        var expediente = _expedienteRepo.ObtenerExpedientePorId(request.IdExpediente)
            ?? throw new NotFoundException("No existe el expediente solicitado.");

        var tramitesAsociados = _tramiteRepo.ObtenerTramitesPorExpedienteId(request.IdExpediente);
        foreach (var tramite in tramitesAsociados)
            _tramiteRepo.EliminarTramite(tramite.Id);

        _expedienteRepo.EliminarExpediente(request.IdExpediente);
        _unidadDeTrabajo.Guardar();

        return new BajaExpedienteResponse(request.IdExpediente, true);
    }
}