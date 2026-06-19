using Aplicacion.Tramites;
using Dominio.Tramites;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura;

public class TramiteRepository : ITramiteRepository
{
    private readonly SgeContext _context;

    public TramiteRepository(SgeContext context)
    {
        _context = context;
    }

    public void AgregarTramite(Tramite tramite)
    {
        _context.Tramites.Add(tramite);
    }

    public void ModificarTramite(Tramite tramite)
    {
        _context.Tramites.Update(tramite);
    }

    public void EliminarTramite(Guid id)
    {
        var tramite = _context.Tramites.Find(id) 
            ?? throw new Exception("Tramite no encontrado");
        _context.Tramites.Remove(tramite);
    }

    public Tramite? ObtenerTramitePorId(Guid id)
    {
        return _context.Tramites.Find(id);
    }

    public IEnumerable<Tramite> ObtenerTodosLosTramites()
    {
        return _context.Tramites.ToList();
    }

    public IEnumerable<Tramite> ObtenerTramitesPorExpedienteId(Guid expedienteId)
    {
        return _context.Tramites
            .Where(t => t.ExpedienteId == expedienteId)
            .ToList();
    }
}