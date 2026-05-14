namespace Consola;

using Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Infraestructura;
using Aplicacion.Autorizacion;
using Dominio.Comun;
//inyeccion de dependencias
public class Program
{
    public static void Main()
    {   //usuarios para testeo
        Guid usuario1 = Guid.NewGuid();
        Guid usuario2 = Guid.NewGuid();
        Guid usuario3 = Guid.NewGuid();
        //repositorios y servicios
        var expedienteRepo = new ExpedienteTxtRepository();
        var tramiteRepo = new TramiteTxtRepository();
        var authorizationService = new AuthorizationService();
        var estadoExpService = new ActualizacionEstadoExpService(expedienteRepo, tramiteRepo);
        //expedientes
        var altaExpedienteUseCase = new AltaExpedienteUseCase(expedienteRepo, authorizationService);
        var bajaExpedienteUseCase = new EliminarExpedienteUseCase(expedienteRepo, tramiteRepo, authorizationService);    
        var modificarExpedienteUseCase = new ModificarExpedienteUseCase(expedienteRepo, authorizationService);
        var modificarCaratulaExpedienteUseCase = new ModificarCaratulaExpedienteUseCase(expedienteRepo, authorizationService);
        var obtenerExpedientesUseCase = new ListarExpedientesUseCase(expedienteRepo, authorizationService);
        //tramites
        var altaTramiteUseCase = new AltaTramiteUseCase(tramiteRepo,expedienteRepo, authorizationService, estadoExpService);
        var bajaTramiteUseCase = new BajaTramiteUseCase(tramiteRepo, estadoExpService, authorizationService);
        var obtenerTramitesUseCase = new ListarTramitesUseCase(tramiteRepo, authorizationService);


        //enums validos:   EscritoPresentado, PaseAEstudio, Despacho, Resolucion, Notificacion, PaseAlArchivo 
        //ejemplo de uso
        // var tramite1 = altaTramiteUseCase.Ejecutar(new AltaTramiteRequest(Guid.NewGuid(), "EscritoPresentado", new ContenidoTramite("Se crea el tramite"), Guid.NewGuid()));
        // var tramite2 = altaTramiteUseCase.Ejecutar(new AltaTramiteRequest(Guid.NewGuid(), "Notificacion", new ContenidoTramite("Se actualiza el tramite"), Guid.NewGuid()));
        
        
        
        Test("crear caratula vacia", () =>
        {
           var solicitud = new AltaExpedienteRequest("", usuario1);
           altaExpedienteUseCase.Ejecutar(solicitud); 
        });
        
        Test("crear expediente con caratula", () =>
        {
           var solicitud = new AltaExpedienteRequest("Expediente de prueba", usuario1);
           var respuesta = altaExpedienteUseCase.Ejecutar(solicitud); 
           Console.WriteLine($"Expediente creado con ID: {respuesta.Id}");
        });
        
        
        
        
        
        static void Test (string accion, Action funcionalidad) //Action representa un metodo
        {
            try
            {
                funcionalidad();
            }
            catch (AuthorizationException ex)
            {
                Console.WriteLine($"Error al {accion}: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Error al {accion}: {ex.Message}");
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"Error al {accion}: {ex.Message}");
            }
        }
    }
}
