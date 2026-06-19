namespace Aplicacion.Expedientes;
using Aplicacion.Autorizacion;
using Aplicacion.Comun;
using Dominio.Autorizacion;
using Aplicacion.Unidad;
public class AltaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public AltaExpedienteUseCase(
        IExpedienteRepository expedienteRepo,
        IAutorizacionService autorizacionService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
            throw new AuthorizationException("No posee permiso para crear un expediente");

        var caratula = new Caratula(request.Caratula);
        var nuevoExpediente = new Expediente(caratula, request.IdUsuario);

        _expedienteRepo.AgregarExpediente(nuevoExpediente);
        _unidadDeTrabajo.Guardar();

        return new AltaExpedienteResponse(nuevoExpediente.Id, caratula.ToString(), nuevoExpediente.FechaCreacion);
    }
}