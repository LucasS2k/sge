namespace Infraestructura;
using Aplicacion.Usuarios;
using Dominio.Autorizacion;
using Dominio.Usuarios;
using Microsoft.EntityFrameworkCore;
public class SgeSqlite
{
    public static void Inicializar(SgeContext context)
{
    if (context.Database.EnsureCreated())
    {
        Console.WriteLine("BASE DE DATOS CREADA CON EXITO");
    }

    using (var transaction = context.Database.BeginTransaction())
    {
        try
        {   if (context.Usuarios.Any()) return;

        Console.WriteLine("Inicializando datos");

            var admin = new Usuario("Administrador", "admin@sge.com", Hash.Calcular("admin123"), true);
            var usuarioAlgunos = new Usuario("Usuario Algunos", "algunos@unlp.com", Hash.Calcular("algunos123"));
            usuarioAlgunos.AgregarPermiso(Permiso.ExpedienteAlta);
            var usuarioSinPermisos = new Usuario("Usuario Sinpermisos", "sinpermisos@unlp.com", Hash.Calcular("sinpermisos123"));

            context.Usuarios.AddRange(admin, usuarioAlgunos, usuarioSinPermisos);
            context.SaveChanges();
            
            transaction.Commit();
            Console.WriteLine("Datos iniciales insertados correctamente");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error al insertar datos: {ex.Message}");
            throw;
        }
    }
}
}