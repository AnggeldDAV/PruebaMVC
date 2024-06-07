using System.Linq.Expressions;

namespace PruebaMVC.Services.Repositorio
{
    public interface IGenericRepositorio<T> where T : class
    {
        List<T> DameTodos();
        T DameUno(int Id);
        bool Borrar(int Id);
        bool Agregar(T element);
        void Modificar(int Id,T element);
        List<T> Filtra(Expression<Func<T, bool>> predicado);
    }
}
