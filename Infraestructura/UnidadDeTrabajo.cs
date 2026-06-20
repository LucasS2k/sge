using Aplicacion.Unidad;
namespace Infraestructura;
//implementacion de la unidad de trabajo, que permite guardar los cambios en la base de datos
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