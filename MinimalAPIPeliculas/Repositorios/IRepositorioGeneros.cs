using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Repositorios
{
    public interface IRepositorioGeneros
    {
        Task<int> Crear(Genero genero);
        Task<Genero?> ObtenerPorId(int id);
        Task<List<Genero>> ObtenerTodos();
        Task<bool> Existe(int id);
        Task Actualizar(Genero genero);
        Task Borrar(int id);
    }
}