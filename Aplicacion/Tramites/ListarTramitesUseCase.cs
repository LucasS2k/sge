namespace Aplicacion.Tramites;
using Aplicacion.Autorizacion;
using Aplicacion.Comun;
using Dominio.Autorizacion;

public class ListarTramitesUseCase
{
     private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacionService;
    public ListarTramitesUseCase(ITramiteRepository tramiteRepo, IAutorizacionService autorizacionService)
    {
        _tramiteRepo = tramiteRepo;
        _autorizacionService = autorizacionService;
    }

    public ListarTramitesResponse Ejecutar(ListarTramitesRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteListar))
        {
            throw new AuthorizationException("no posee permiso para consultar tramites");
        }
        var entidades = _tramiteRepo.ObtenerTodosLosTramites();
        var datos = entidades.Select(t => new TramiteResponse(
           t.Id, t.Id, t.Etiqueta.ToString()??"sin etiqueta", t.Contenido.ToString()??"sin contenido", t.FechaCreacion, t.UsuarioUltimoCambio
           )).ToList();//retornamos una lista;
        return new ListarTramitesResponse(datos);
    }

}