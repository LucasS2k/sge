using Aplicacion.Expedientes;
using Dominio.Expedientes;
using Microsoft.EntityFrameworkCore;
namespace Infraestructura;

public class ExpedienteRepository : IExpedienteRepository
{
    private readonly SgeContext _context;

    public ExpedienteRepository(SgeContext context)
    {
        _context = context;
    }

    public void AgregarExpediente(Expediente expediente)
    {
        _context.Expedientes.Add(expediente);
    }

    public void EliminarExpediente(Guid id)
    {
        var expediente = _context.Expedientes.Find(id) 
            ?? throw new Exception("Expediente no encontrado");
        _context.Expedientes.Remove(expediente);
    }

    public IEnumerable<Expediente> ObtenerTodosLosExpedientes()
    {
        return _context.Expedientes.ToList();
    }

    public Expediente? ObtenerExpedientePorId(Guid id)
    {
        return _context.Expedientes.Find(id);
    }

    public void ModificarExpediente(Expediente expediente)
    {
        _context.Expedientes.Update(expediente);
    }
}