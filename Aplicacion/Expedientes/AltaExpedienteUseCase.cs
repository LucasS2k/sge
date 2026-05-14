
namespace Aplicacion.Expedientes;
using Dominio.Autorizacion;
public class AltaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;

    public AltaExpedienteUseCase(IExpedienteRepository expedienteRepo, IAutorizacionService autorizacionService)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
    }

    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if(!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new Exception(" crear un expediente");
        }
        var caratula = new Caratula(request.Caratula);
        var nuevoExpediente = new Expediente(caratula, request.IdUsuario);
        _expedienteRepo.AgregarExpediente(nuevoExpediente);

        return new AltaExpedienteResponse(nuevoExpediente.Id, caratula.ToString(), nuevoExpediente.FechaCreacion);
    }
}