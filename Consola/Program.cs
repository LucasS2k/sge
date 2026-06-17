namespace WebApi;

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
        //guardado de guid de expediente valido
        Guid idExpedienteValido = Guid.Empty;
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
        Test("Crear expediente valido", () =>
        {
            var solicitud = new AltaExpedienteRequest("Expediente valido", usuario1);
            var respuesta = altaExpedienteUseCase.Ejecutar(solicitud); 
            idExpedienteValido = respuesta.Id; 
            Console.WriteLine($"Id generado: {idExpedienteValido}"); //este id lo usamos para testear
        });

        Test("Crear expediente caratula vacia", () =>
        {
           var solicitud = new AltaExpedienteRequest("", usuario1);
           altaExpedienteUseCase.Ejecutar(solicitud); 
        });
        
        Test("Crear expediente con caratula", () =>
        {
           var solicitud = new AltaExpedienteRequest("Expediente de prueba", usuario1);
           var respuesta = altaExpedienteUseCase.Ejecutar(solicitud); 
           Console.WriteLine($"Expediente creado con ID: {respuesta.Id}");
        });
        
        Test ("Modificar caratula del expediente existente", () =>
        {
            var solicitud = new ModificarExpedienteRequest(
                idExpedienteValido,"Caratula modificada", usuario1
            );
            modificarCaratulaExpedienteUseCase.Ejecutar(solicitud);
            Console.WriteLine("Caratula modificada exitosamente");
        });

        Test ("Listar expedientes",()=>
        {
            var response = obtenerExpedientesUseCase.Ejecutar(new ListarExpedientesRequest(usuario1));
            if (!response.Expedientes.Any())
            {
                Console.WriteLine("No hay expedientes");
            }else
            {
                foreach(var e in response.Expedientes)
                {
                    Console.WriteLine($"id: {e.Id} caratula: {e.Caratula} estado: {e.Estado} ");
                }
            }
        });
        
        Test("Crear tramite", () =>{
        var solicitud = new AltaTramiteRequest(
        idExpedienteValido,           
        "EscritoPresentado",         
        new ContenidoTramite("Contenido de ejemplo"), 
        usuario1
        );
            altaTramiteUseCase.Ejecutar(solicitud);
            Console.WriteLine("Tramite creado exitosamente");
        });
        
        Test ("Tramite a un Id desconocido", () =>
        {
            var solicitud = new AltaTramiteRequest(
        new Guid(), //Generamos un guid nuevo que no coincide con ningun expediente creado        
        "EscritoPresentado",         
        new ContenidoTramite("Contenido de ejemplo"), 
        usuario1
        );
            altaTramiteUseCase.Ejecutar(solicitud);
        });

        Test ("Listar tramites", () =>
        {
            var response = obtenerTramitesUseCase.Ejecutar(new ListarTramitesRequest(usuario1));
            if (!response.Tramites.Any())
            {
                Console.WriteLine("No hay tramites para listar");
            } else
            {
                foreach (var t in response.Tramites)
                {
                    Console.WriteLine($"Id: {t.Id} Etiqueta: {t.Etiqueta} usuario: {t.IdUsuario} fecha creacion: {t.FechaCreacion}");
                }
            }
        });
        static void Test (string accion, Action funcionalidad) //Action representa un metodo
        {
            try
            {
                funcionalidad();
                Console.WriteLine("Exitoso");
            }
            catch (AuthorizationException ex)
            {
                Console.WriteLine($"Error al intentar: {accion}: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Error al intentar: {accion}: {ex.Message}");
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"Error al intentar: {accion}: {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("Excepcion no manejada");
            }
        }
    }
}
