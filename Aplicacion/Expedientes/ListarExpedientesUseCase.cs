namespace Aplicacion.Expedientes;
using Aplicacion.Autorizacion;
using Dominio.Autorizacion;
using Dominio.Comun;

public class ListarExpedientesUseCase
{
    private readonly IExpedienteRepository _expedienteRepo;
    private readonly IAutorizacionService _autorizacionService;

    public ListarExpedientesUseCase(IExpedienteRepository expedienteRepo, IAutorizacionService autorizacionService)
    {
        _expedienteRepo = expedienteRepo;
        _autorizacionService = autorizacionService;
    }

    public ListarExpedientesResponse Ejecutar(ListarExpedientesRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteListar))
        {
            throw new AuthorizationException("consultar expedientes");
        }
        var entidades = _expedienteRepo.ObtenerTodosLosExpedientes();
        var datos = entidades.Select(e => new ExpedienteResponse(
            e.Id, e.Caratula.ToString(), 
            e.Estado.ToString(), 
            e.FechaCreacion, 
            e.UsuarioUltimoCambio)).ToList();//retornamos una lista;
        return new ListarExpedientesResponse(datos);
    }
}