namespace Aplicacion.Tramites;

using Dominio.Comun;
using Dominio.Autorizacion;
public class BajaTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly ActualizacionEstadoExpService _estadoService;
    private readonly IAutorizacionService _authService;

    public BajaTramiteUseCase(ITramiteRepository tramiteRepo, ActualizacionEstadoExpService estadoService,
        IAutorizacionService authService)
    {
        _tramiteRepo = tramiteRepo;
        _estadoService = estadoService;
        _authService = authService;
    }

    public void Ejecutar(BajaTramiteRequest request)
    {
        if (!_authService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja))
        {
            throw new AuthorizationException(" Eliminar tramite");
        }
        var tramite = _tramiteRepo.ObtenerTramitePorId(request.IdTramite) 
            ?? throw new NotFoundException("No existe el tramite");
        _tramiteRepo.EliminarTramite(request.IdTramite);
        _estadoService.Ejecutar(tramite.ExpedienteId, request.IdUsuario);
    }
}