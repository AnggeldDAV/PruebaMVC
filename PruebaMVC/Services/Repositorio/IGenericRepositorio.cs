using System.Linq.Expressions;

namespace PruebaMVC.Services.Repositorio
{
    public interface IGenericRepositorio<T> where T : class
    {
        Task<List<T>> DameTodos();
        Task<T?> DameUno(int Id);
        Task<bool> Borrar(int Id);
        Task<bool> Agregar(T element);
        Task Modificar(int Id,T element);
        Task<List<T>> Filtra(Expression<Func<T, bool>> predicado);
    }
}
