namespace Aplicacion.Expedientes;
using Dominio.Autorizacion;
using Dominio.Comun;
using Aplicacion.Comun;
using Aplicacion.Unidad;
using Dominio.Expedientes;
public class ModificarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarExpedienteUseCase(
        IExpedienteRepository expedienteRepo,
        IAutorizacionService autorizacionService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
public ModificarExpedienteResponse Ejecutar(ModificarEstadoExpedienteRequest request)
{
    if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        throw new AuthorizationException("No posee permisos para modificar expedientes");

    var expediente = _expedienteRepo.ObtenerExpedientePorId(request.IdExpediente)
        ?? throw new NotFoundException("No existe el expediente solicitado.");
    //forzar conversion
    if (!Enum.TryParse<Dominio.Expedientes.EstadoExpediente>(request.NuevoEstado, true, out var nuevoEstadoEnum))
    {
        throw new DomainException($"El estado '{request.NuevoEstado}' No es un estado válido");
    }

    expediente.CambiarEstado(nuevoEstadoEnum, request.IdUsuario);

    _expedienteRepo.ModificarExpediente(expediente);
    _unidadDeTrabajo.Guardar();

    return new ModificarExpedienteResponse(request.IdExpediente, true);
}
}