namespace Aplicacion.Tramites;
using Aplicacion.Expedientes;
using Dominio.Tramites;
using Dominio.Autorizacion;

public class AgregarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IExpedienteRepository _expedienteRepo; // Lo necesitamos para actualizar el estado
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpService _estadoService;

    public AgregarTramiteUseCase(
        ITramiteRepository tramiteRepo, 
        IExpedienteRepository expedienteRepo,
        IAutorizacionService autorizacionService,
        ActualizacionEstadoExpService estadoService)
    {
        _tramiteRepo = tramiteRepo;
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
    }

    public void Ejecutar(AltaTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta))
            throw new AuthorizationException("crear tramite");

        var nuevoTramite = new Tramite(request.IdExpediente, Enum.TryParse<EstadoTramite>(request.Etiqueta, out var estado) ? estado : throw new ArgumentException("Estado inválido"), request.Contenido, request.IdUsuario);
        _tramiteRepo.AgregarTramite(nuevoTramite);
        _estadoService.Ejecutar(request.IdExpediente, request.IdUsuario);
    }
}