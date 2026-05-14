namespace Aplicacion.Expedientes;
using Dominio.Autorizacion;
using Dominio.Comun;
public class ModificarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;

    public ModificarExpedienteUseCase(IExpedienteRepository expedienteRepo, IAutorizacionService autorizacionService)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
    }

    public ModificarExpedienteResponse Ejecutar(ModificarExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
            throw new AuthorizationException("No posee permisos para modificar expedientes");

        var expediente = _expedienteRepo.ObtenerExpedientePorId(request.IdExpediente) 
            ?? throw new NotFoundException("No existe el expediente solicitado");
        _expedienteRepo.ModificarExpediente(expediente);
        return new ModificarExpedienteResponse(request.IdExpediente, true);
    }
}