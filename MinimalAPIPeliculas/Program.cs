using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Repositorios;
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




var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseOutputCache();

var endpointsGeneros = app.MapGroup("/generos");
endpointsGeneros.MapGet("/", async (IRepositorioGeneros repositorio) =>
{
    return await repositorio.ObtenerTodos();
});

endpointsGeneros.MapGet("/{id:int}", async (int id, IRepositorioGeneros repositorio) =>
{
    var genero = await repositorio.ObtenerPorId(id);

    if(genero is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genero);
});

endpointsGeneros.MapPost("/", async (Genero genero,
        IRepositorioGeneros repositorioGeneros) =>
{
    var id = await repositorioGeneros.Crear(genero);
    return TypedResults.Created($"/generos/{id}", genero);
});


endpointsGeneros.MapPut("/{id:int}", async (int id, Genero genero, IRepositorioGeneros repositorio,
    IOutputCacheStore outputCacheStore) =>
{
    var existe = await repositorio.Existe(id);
    if (!existe)
    {
        return Results.NotFound();
    }
    await repositorio.Actualizar(genero);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return Results.NoContent();
});


endpointsGeneros.MapDelete("/{id:int}", async (int id, IRepositorioGeneros repositorio,
    IOutputCacheStore outputCacheStore) =>
{
    var existe = await repositorio.Existe(id);
    if (!existe)
    {
        return Results.NotFound();
    }
    await repositorio.Borrar(id);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return Results.NoContent();
});

app.Run();
