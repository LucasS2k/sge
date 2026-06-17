using Aplicacion;
using Aplicacion.Comun;
using Aplicacion.Expedientes;
using Aplicacion.Tramites;
using Aplicacion.Usuarios;
using Infraestructura;
using Infraestructura.Repositorios;
using Infraestructura.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;
using WebApi;
using WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// BLOQUE 1: CONFIGURACIÓN DE SERVICIOS (El Contenedor DI)
// ====================================================================

// A. Base de Datos
var connectionString = builder.Configuration.GetConnectionString("SgeDb");
builder.Services.AddDbContext<SgeContext>(opciones =>
    opciones.UseSqlite(connectionString));

// B. Patrón Unit of Work y Repositorios
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<ITramiteRepository, TramiteRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// C. Seguridad
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<ITokenProvider, JwtTokenProvider>();

// D. Casos de Uso — Expedientes
builder.Services.AddScoped<AltaExpedienteUseCase>();
builder.Services.AddScoped<BajaExpedienteUseCase>();
builder.Services.AddScoped<ModificarCaratulaExpedienteUseCase>();
builder.Services.AddScoped<ModificarExpedienteUseCase>();
builder.Services.AddScoped<ListarExpedientesUseCase>();
builder.Services.AddScoped<ObtenerExpedienteUseCase>();

// D. Casos de Uso — Trámites
builder.Services.AddScoped<AltaTramiteUseCase>();
builder.Services.AddScoped<BajaTramiteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();
builder.Services.AddScoped<ListarTramitesUseCase>();
builder.Services.AddScoped<ListarTramitesPorExpedienteUseCase>();
builder.Services.AddScoped<ActualizacionEstadoExpService>();

// D. Casos de Uso — Usuarios
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<ModificarMisDatosUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();
builder.Services.AddScoped<BajaUsuarioUseCase>();
builder.Services.AddScoped<ModificarPermisosUsuarioUseCase>();

// E. JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// F. Manejo de excepciones y documentación
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Ingresá el token JWT obtenido en /usuarios/login"
        });
        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, ct) =>
    {
        var endpoint = context.Description.ActionDescriptor.EndpointMetadata;
        var requiereAuth = endpoint.OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>().Any();
        if (requiereAuth)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new(){{new OpenApiSecurityScheme{Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }},
                        Array.Empty<string>()}}};}
        return Task.CompletedTask;
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    SgeSqlite.Inicializar(context);
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options.WithPreferredScheme("Bearer"));
}
//endpoints
app.MapUsuariosEndpoints();
app.MapExpedientesEndpoints();
app.MapTramitesEndpoints();

app.Run();