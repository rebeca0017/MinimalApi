﻿using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.Entidades;
using System.Data;
using System.Data.Common;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioGeneros : IRepositorioGeneros
    {
        private readonly string? connectionString;

        public RepositorioGeneros(IConfiguration configuration)
        {
            connectionString
                = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Genero>> ObtenerTodos()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var generos = await conexion.QueryAsync<Genero>("Generos_ObtenerTodos", 
                    commandType: CommandType.StoredProcedure);

                return generos.ToList();
            }
        }

        public async Task<Genero?>ObtenerPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var genero = await conexion.QueryFirstOrDefaultAsync<Genero>("Generos_ObtenerPorId", new {id}, 
                    commandType: CommandType.StoredProcedure);

                return genero;
            }

        }
        public async Task<int> Crear(Genero genero)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>("Generos_Crear", 
                    new { genero.Nombre },
                    commandType: CommandType.StoredProcedure);

                genero.Id = id;
                return id;
            }
        }

       
        public async Task<bool> Existe(int id)
        {
            using(var conexion =new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>("Genero_Existe", new {id},
                    commandType: CommandType.StoredProcedure);
                return existe; 
            }
        }

        public async Task<bool>Existe (int id, string nombre)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion
                    .QuerySingleAsync<bool>("Generos_ExistePorIdYNombre",
                     new { id, nombre },
                     commandType: CommandType.StoredProcedure);
                return existe; 
            }
        }

        public async Task<List<int>> Existen(List<int> ids)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            foreach (var id in ids)
            {
                dt.Rows.Add(id);
            }

            using (var conexion = new SqlConnection(connectionString))
            {
                var idsGenerosExistentes = await conexion
                    .QueryAsync<int>("Generos_ObtenerVariosPorId", new { generosIds = dt },
                    commandType: CommandType.StoredProcedure);
                return idsGenerosExistentes.ToList();
            }
        }
 

        public async Task Actualizar(Genero genero )
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Genero_Actualizar", genero,
                    commandType: CommandType.StoredProcedure);
            }
            }

        public async Task Borrar(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Generos_Borrar", new {id}, 
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
