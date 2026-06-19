namespace Aplicacion.Tramites;
using Dominio.Autorizacion;
using Aplicacion.Autorizacion;
using Aplicacion.Comun;
using Aplicacion.Unidad;
public class BajaTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly ActualizacionEstadoExpService _estadoService;
    private readonly IAutorizacionService _authService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public BajaTramiteUseCase(
        ITramiteRepository tramiteRepo,
        ActualizacionEstadoExpService estadoService,
        IAutorizacionService authService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _tramiteRepo = tramiteRepo;
        _estadoService = estadoService;
        _authService = authService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public BajaTramiteResponse Ejecutar(BajaTramiteRequest request)
    {
        if (!_authService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja))
            throw new AuthorizationException("No posee permiso para eliminar trámites");

        var tramite = _tramiteRepo.ObtenerTramitePorId(request.IdTramite)
            ?? throw new NotFoundException("No existe el trámite");

        _tramiteRepo.EliminarTramite(request.IdTramite);
        _estadoService.Ejecutar(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();

        return new BajaTramiteResponse(request.IdTramite, true);
    }
}