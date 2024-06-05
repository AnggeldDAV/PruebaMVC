using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;

namespace PruebaMVC.Services.Repositorio
{
    public class EFConciertoRepositorio :IConciertosRepositorio
    {
        private readonly GrupoCContext _context = new();
        public List<Concierto> DameTodos()
        {
            return _context.Conciertos.AsNoTracking().ToList();
        }

        public Concierto? DameUno(int Id)
        {
            return _context.Conciertos.Find(Id);
        }

        public bool Borrar(int Id)
        {
            if (DameUno(Id) != null)
            {
                _context.Conciertos.Remove(DameUno(Id));
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Agregar(Concierto concierto)
        {
            if (DameUno(concierto.Id) != null)
            {

                return false;
            }
            else
            {
                _context.Conciertos.Add(concierto);
                _context.SaveChanges();
                return true;
            }
        }

        public void Modificar(int Id, Concierto concierto)
        {
            var respuesta = DameUno(Id);
            if (respuesta != null)
            {
                Borrar(Id);
                _context.SaveChanges();
            }
            Agregar(concierto);
            _context.SaveChanges();
        }
    }
}
