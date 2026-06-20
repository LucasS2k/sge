namespace Infraestructura;
using Microsoft.EntityFrameworkCore;
using Dominio.Usuarios;
using Dominio.Autorizacion;
using Dominio.Tramites;
using Aplicacion.Usuarios;

public class SgeContext : DbContext
{
    public DbSet<Expediente> Expedientes => Set<Expediente>();
    public DbSet<Tramite> Tramites => Set<Tramite>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public SgeContext(DbContextOptions<SgeContext> options) : base(options) { }
//REVISAR ANTES DE ENTREGAR
// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// {
//     
//     optionsBuilder.UseSqlite(@"Data Source=C:\SGE\sge.db;Cache=Shared;Pooling=False");
// }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        //Expediente
        modelBuilder.Entity<Expediente>(e =>
        {
            e.HasKey(x => x.Id);
            e.ComplexProperty(x => x.Caratula, c =>
            {
                c.Property(v => v.Valor).HasColumnName("Caratula").IsRequired();
            });
            e.Property(x => x.Estado).HasConversion<string>(); //no se le puede enviar enum directamente
            e.Property(x => x.FechaCreacion).IsRequired();
            e.Property(x => x.FechaUltimaModificacion).IsRequired();
            e.Property(x => x.UsuarioUltimoCambio).IsRequired();
        });

        //tramite
        modelBuilder.Entity<Tramite>(t =>
        {
            t.HasKey(x => x.Id);
            t.Property(x => x.ExpedienteId).IsRequired();
            t.Property(x => x.Etiqueta).HasConversion<string>();
            t.ComplexProperty(x => x.Contenido, c =>
            {
                c.Property(v => v.Valor).HasColumnName("Contenido").IsRequired();
            });
            t.Property(x => x.FechaCreacion).IsRequired();
            t.Property(x => x.FechaUltimaModificacion).IsRequired();
            t.Property(x => x.UsuarioUltimoCambio).IsRequired();
        });

        //usuario
        modelBuilder.Entity<Usuario>(u =>
        {
            u.HasKey(x => x.Id);
            u.Property(x => x.Nombre).IsRequired();
            u.Property(x => x.CorreoElectronico).IsRequired();
            u.Property(x => x.ContrasenaHash).IsRequired();
            u.Property(x => x.EsAdministrador).IsRequired();

            u.PrimitiveCollection(x => x.Permisos)
             .HasColumnName("Permisos")
             .ElementType(b => b.HasConversion<string>());
        });
    }
}