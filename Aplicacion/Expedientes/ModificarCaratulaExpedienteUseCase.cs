namespace Aplicacion.Expedientes;
using Dominio;
using Dominio.Autorizacion;
using Dominio.Comun;
public class ModificarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;

    public ModificarCaratulaExpedienteUseCase(IExpedienteRepository expedienteRepo, IAutorizacionService autorizacionService)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarExpedienteRequest request)
    {
        var nuevaCaratula = new Caratula(request.NuevaCaratula);
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
            throw new AuthorizationException("No posee permisos para modificar expedientes");

        var expediente = _expedienteRepo.ObtenerExpedientePorId(request.IdExpediente) 
            ?? throw new NotFoundException("No existe el expediente solicitado");
        
        expediente.ModificarCaratula(nuevaCaratula, request.IdUsuario);
        _expedienteRepo.ModificarExpediente(expediente);
        return new ModificarCaratulaExpedienteResponse(request.IdExpediente, true);
    }
}