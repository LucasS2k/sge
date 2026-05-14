namespace Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Dominio.Autorizacion;
public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacionService;

    public EliminarExpedienteUseCase(
        IExpedienteRepository expedienteRepo, 
        ITramiteRepository tramiteRepo, 
        IAutorizacionService autorizacionService)
    {
        _expedienteRepo = expedienteRepo;
        _tramiteRepo = tramiteRepo;
        _autorizacionService = autorizacionService;
    }

    public void Ejecutar(BajaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AuthorizationException("");
        }
        var tramitesAsociados = _tramiteRepo.ObtenerTramitesPorExpedienteId(request.IdExpediente);
        foreach (var tramite in tramitesAsociados)
        {
            _tramiteRepo.EliminarTramite(tramite.Id);
        }
        _expedienteRepo.EliminarExpediente(request.IdExpediente);
    }
}