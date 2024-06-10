using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;

namespace PruebaMVC.Services.Repositorio
{
    public class EFGenericRepositorio<T> : IGenericRepositorio<T> where T : class
    {
        private readonly GrupoCContext _context = new();
        public async Task<List<T>> DameTodos()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> DameUno(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<bool> Borrar(int Id)
        { 
            var elemento = await DameUno(Id);
            _context.Set<T>().Remove(elemento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Agregar(T element)
        {
            await _context.Set<T>().AddAsync(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public async void Modificar(int Id, T element)
        {
            _context.Entry(element).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> Filtra(Expression<Func<T, bool>> predicado)
        {
            return _context.Set<T>().Where<T>(predicado).ToList();
        }
    }
}
