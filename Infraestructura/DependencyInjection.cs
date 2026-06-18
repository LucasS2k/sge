using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Unidad;
using Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Aplicacion.Usuarios;

namespace Infraestructura;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SgeDb");
        services.AddDbContext<SgeContext>(opciones =>
            opciones.UseSqlite(connectionString));
        services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

        services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
        services.AddScoped<ITramiteRepository, TramiteRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IAutorizacionService, AutorizacionService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        return services;
    }
}