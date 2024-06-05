using PruebaMVC.Models;

namespace PruebaMVC.Services.Repositorio
{
    public interface IConciertosRepositorio
    {
        List<Concierto> DameTodos();
        Concierto? DameUno(int Id);
        bool Borrar(int Id);
        bool Agregar(Concierto concierto);
        void Modificar(int Id, Concierto concierto);
    }
}
