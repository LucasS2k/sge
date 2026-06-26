namespace Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Aplicacion.Comun;

public class ObtenerConTramitesUseCase
{
    private readonly IExpedienteRepository _repo;

    public ObtenerConTramitesUseCase(IExpedienteRepository repo) => _repo = repo;

    public ObtenerExpedienteResponse Ejecutar(ObtenerExpedienteRequest request)
{
    var exp = _repo.ObtenerConTramites(request.IdExpediente) 
              ?? throw new NotFoundException("Expediente no encontrado");

    return new ObtenerExpedienteResponse(
        exp.Id,
        exp.Caratula.ToString(),
        exp.Estado.ToString(),
        exp.FechaCreacion,
        exp.UsuarioUltimoCambio,
        //traemos todos los tramites asociados
        exp.Tramites.Select(t => new TramiteResponse(
            t.Id,
            t.ExpedienteId,
            t.Etiqueta?.ToString() ?? "SinEtiqueta",
            t.Contenido.ToString(),
            t.FechaCreacion,
            t.UsuarioUltimoCambio
        )).ToList()
    );
}
}