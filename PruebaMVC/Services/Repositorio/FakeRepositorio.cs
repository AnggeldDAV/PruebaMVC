using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;

namespace PruebaMVC.Services.Repositorio
{
    public class FakeRepositorio : IConciertosRepositorio
    {
        List<Concierto> ListaConciertos = [];
        public FakeRepositorio()
        {
            Concierto MiConcierto = new Concierto()
            {
                Titulo = "FakeTitulo1",
                Precio = 13,
                Lugar = "Dinamarca",
                Fecha = new DateTime(2001, 2, 12, 12, 12, 00),
                Id = 1,
                Genero = "Funk"
            };
            ListaConciertos.Add(MiConcierto);
            Concierto MiConcierto2 = new Concierto()
            {
                Titulo = "FakeTitulo2",
                Precio = 100,
                Lugar = "Albacete",
                Fecha = new DateTime(2001, 4, 12, 12, 12, 00),
                Id = 2,
                Genero = "Metal"
            };
            ListaConciertos.Add(MiConcierto2);
            Concierto MiConcierto3 = new Concierto()
            {
                Titulo = "FakeTitulo3",
                Precio = 200,
                Lugar = "Finlandia",
                Fecha = new DateTime(2001, 11, 12, 12, 12, 00),
                Id = 3,
                Genero = "Heavy Metal"
            };
            ListaConciertos.Add(MiConcierto3);
            Concierto MiConcierto4 = new Concierto()
            {
                Titulo = "FakeTitulo4",
                Precio = 50,
                Lugar = "Asturias",
                Fecha = new DateTime(2022, 9, 12, 12, 12, 00),
                Id = 4,
                Genero = "Rock"
            };
            ListaConciertos.Add(MiConcierto4);
            Concierto MiConcierto5 = new Concierto()
            {
                Titulo = "FakeTitulo5",
                Precio = 12,
                Lugar = "Suecia",
                Fecha = new DateTime(2003, 1, 12, 23, 12, 00),
                Id = 5,
                Genero = "Jamaicano"
            };
            ListaConciertos.Add(MiConcierto5);
        }
        public List<Concierto> DameTodos()
        {
            return this.ListaConciertos;
        }

        public Concierto? DameUno(int Id)
        {
            return this.ListaConciertos.FirstOrDefault(x => x.Id == Id);
        }

        public bool Borrar(int Id)
        {
            ListaConciertos.Remove(DameUno(Id));
            return true;
        }

        public bool Agregar(Concierto concierto)
        {
            ListaConciertos.Add(concierto);
            return true;
        }
        public void Modificar(int Id, Concierto concierto)
        {
            Borrar(Id);
            Agregar(concierto);
        }
    }

}
