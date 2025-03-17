using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.Entidades;
using System.Data.Common;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioGeneros : IRepositorioGeneros
    {
        private readonly string? connectionString;

        public RepositorioGeneros(IConfiguration configuration)
        {
            connectionString
                = configuration.GetConnectionString("DefaultConecction");
        }

        public async Task<List<Genero>> ObtenerTodos()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var generos = await conexion.QueryAsync<Genero>(@"
                                                                SELECT Id, Nombre
                                                                FROM Generos 
                                                                ORDER BY  Nombre");

                return generos.ToList();
            }
        }

        public async Task<Genero?>ObtenerPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var genero = await conexion.QueryFirstOrDefaultAsync<Genero>(@"Select Id, Nombre from Generos WHERE Id=@Id", new {id});

                return genero;
            }

        }
        public async Task<int> Crear(Genero genero)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>(@"
                    INSERT INTO Generos (Nombre) 
                    VALUES (@Nombre);
                    SELECT SCOPE_IDENTITY();", genero);

                genero.Id = id;
                return id;
            }
        }

       
        public async Task<bool> Existe(int id)
        {
            using(var conexion =new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>(@"
                                                IF EXISTS (SELECT 1 FROM Generos WHERE Id = @Id)
                                                    SELECT 1
                                                ELSE
                                                    SELECT 0 
                                                ", new {id});
                return existe; 
            }
        }

        public async Task Actualizar(Genero genero )
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"UPDATE Generos
                                                                SET Nombre = @Nombre
                                                                WHERE Id = @Id", genero);
            }
            }

        public async Task Borrar(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"DELETE Generos WHERE Id= @id", new {id});
            }
        }
    }
}
