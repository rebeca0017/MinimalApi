using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.Endpoint;
using MinimalAPIPeliculas.Endpoints;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Repositorios;
using MinimalAPIPeliculas.Servicios;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos");


builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
            configuracion.WithOrigins(origenesPermitidos!).AllowAnyHeader().AllowAnyMethod();
    });
    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
 });

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();
builder.Services.AddScoped<IRepositorioPeliculas, RepositorioPeliculas>();
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();

builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();

app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId: int}/comentarios").MapComentarios();

app.Run();
