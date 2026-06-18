using Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Aplicacion.Usuarios;
using Microsoft.Extensions.DependencyInjection;
namespace Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        //Expedientes
        services.AddScoped<AltaExpedienteUseCase>();
        services.AddScoped<BajaExpedienteUseCase>();
        services.AddScoped<ModificarCaratulaExpedienteUseCase>();
        services.AddScoped<ModificarExpedienteUseCase>();
        services.AddScoped<ListarExpedientesUseCase>();
        //Tramites
        services.AddScoped<AltaTramiteUseCase>();
        services.AddScoped<BajaTramiteUseCase>();
        services.AddScoped<ListarTramitesUseCase>();
        services.AddScoped<ActualizacionEstadoExpService>();
        //Usuarios
        services.AddScoped<RegistrarUsuarioUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<ModificarMisDatosUseCase>();
        services.AddScoped<ListarUsuariosUseCase>();
        services.AddScoped<BajaUsuarioUseCase>();
        services.AddScoped<ModificarPermisosUsuarioUseCase>();

        return services;
    }
}