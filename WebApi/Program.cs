using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using WebApi;
using Microsoft.EntityFrameworkCore;
using WebApi.Endpoints;
using Aplicacion;
using Infraestructura;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {   //valida el emisor
            ValidateIssuer = true,
            //valida el destinatario
            ValidateAudience = true,
            //valida la vida del token
            ValidateLifetime = true,
            //valida quien firma el token
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
var connectionString = builder.Configuration.GetConnectionString("SgeDb");

//REVISAR
var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGE.sqlite");
var connectionStringFinal = $"Data Source={dbPath};Cache=Shared;Pooling=False";
builder.Services.AddDbContext<SgeContext>(opciones =>
    opciones.UseSqlite(connectionStringFinal));
//Servicios
builder.Services.AddInfraestructura(builder.Configuration);
builder.Services.AddCasosDeUso();
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorDeExcepciones>();
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("SGE")
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
//Forzar redirect
app.MapGet("/", () => Results.Redirect("/scalar/v1"));
using (var scope = app.Services.CreateScope())
{   var services = scope.ServiceProvider;
    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    SgeSqlite.Inicializar(context);
}
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapUsuariosEndpoints();
app.MapExpedientesEndpoints();
app.MapTramitesEndpoints();
app.Run();