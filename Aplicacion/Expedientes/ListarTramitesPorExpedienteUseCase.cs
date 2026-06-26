namespace Aplicacion.Expedientes;
using Aplicacion.Tramites; 
using Aplicacion.Comun;      
using System.Collections.Generic;
using System.Linq;

public class ListarTramitesPorExpedienteUseCase
{
    private readonly IExpedienteRepository _repo;

    public ListarTramitesPorExpedienteUseCase(IExpedienteRepository repo) 
    {
        _repo = repo;
    }

    public IEnumerable<TramiteResponse> Ejecutar(Guid idExpediente)
    {
        var exp = _repo.ObtenerConTramites(idExpediente) 
                  ?? throw new NotFoundException("Expediente no encontrado");
        return exp.Tramites.Select(t => new TramiteResponse(
            t.Id,
            t.ExpedienteId,
            t.Etiqueta?.ToString() ?? "SinEtiqueta",
            t.Contenido.Valor, 
            t.FechaCreacion,
            t.UsuarioUltimoCambio
        )).ToList();
    }
}