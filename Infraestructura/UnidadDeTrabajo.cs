using Aplicacion.Unidad;
namespace Infraestructura;
public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SgeContext _context;
    
    public UnidadDeTrabajo(SgeContext context)
    {
        _context = context;
    }
    public void Guardar()
    {
        _context.SaveChanges(); 
    }
}