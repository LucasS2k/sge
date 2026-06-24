namespace Aplicacion.Expedientes;
using Dominio.Autorizacion;
using Aplicacion.Autorizacion;
using Aplicacion.Comun;
using Aplicacion.Unidad;
public class ModificarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarCaratulaExpedienteUseCase(
        IExpedienteRepository expedienteRepo,
        IAutorizacionService autorizacionService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
            throw new AuthorizationException("No posee permisos para modificar expedientes");

        var expediente = _expedienteRepo.ObtenerExpedientePorId(request.IdExpediente)
            ?? throw new NotFoundException("No existe el expediente solicitado");

        var nuevaCaratula = new Caratula(request.NuevaCaratula);
        expediente.ModificarCaratula(nuevaCaratula, request.IdUsuario);

        _expedienteRepo.ModificarExpediente(expediente);
        _unidadDeTrabajo.Guardar();

        return new ModificarCaratulaExpedienteResponse(request.IdExpediente, true);
    }
}