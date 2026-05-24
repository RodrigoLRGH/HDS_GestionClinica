using Presentacion.Components;
using Infraestructura.Data;
using Infraestructura.UnitOfWork;
using Aplicacion.Abstracciones;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Inyecciones;
using Presentacion.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrar DbContext
builder.Services.AddDbContext<ContextoECE>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionBD")));
// Registra UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AgregarAplicacion();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
//Toastr
builder.Services.AddScoped<IToastrService, ToastrService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Después de app.Build() y ANTES de app.MapControllers()
app.UseCors("AllowFrontend");
app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
