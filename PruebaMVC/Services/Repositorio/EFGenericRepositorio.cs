using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;

namespace PruebaMVC.Services.Repositorio
{
    public class EFGenericRepositorio<T> : IGenericRepositorio<T> where T : class
    {
        public readonly GrupoCContext _context = new();
        public List<T> DameTodos()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public T DameUno(int Id)
        {
            return _context.Set<T>().Find(Id);
        }

        public bool Borrar(int Id)
        {
            _context.Set<T>().Remove(DameUno((int)Id));
            _context.SaveChanges();
            return true;
        }

        public bool Agregar(T element)
        {
            _context.Set<T>().Add(element);
            _context.SaveChanges();
            return true;
        }

        public void Modificar(int Id, T element)
        {
            _context.Entry(element).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<T> Filtra(Expression<Func<T, bool>> predicado)
        {
            return _context.Set<T>().Where<T>(predicado).ToList();
        }
    }
}
