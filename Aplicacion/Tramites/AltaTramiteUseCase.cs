namespace Aplicacion.Tramites;
using Aplicacion.Expedientes;
using Dominio.Tramites;
using Dominio.Autorizacion;
using Aplicacion.Comun;
using Aplicacion.Unidad;
public class AltaTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpService _estadoService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public AltaTramiteUseCase(
        ITramiteRepository tramiteRepo,
        IExpedienteRepository expedienteRepo,
        IAutorizacionService autorizacionService,
        ActualizacionEstadoExpService estadoService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _tramiteRepo = tramiteRepo;
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public AltaTramiteResponse Ejecutar(AltaTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta))
            throw new AuthorizationException("No posee permiso para crear tramites");

        if (!Enum.TryParse<EstadoTramite>(request.Etiqueta, out var estado))
            throw new DomainException("Etiqueta de tramite invalida");

        var nuevoTramite = new Tramite(request.IdExpediente, estado, request.Contenido, request.IdUsuario);
        _tramiteRepo.AgregarTramite(nuevoTramite);

        _estadoService.Ejecutar(request.IdExpediente, request.IdUsuario);
        _unidadDeTrabajo.Guardar();

        return new AltaTramiteResponse(
            nuevoTramite.Id,
            request.IdExpediente,
            request.Etiqueta,
            request.Contenido.ToString(),
            nuevoTramite.FechaCreacion);
    }
}