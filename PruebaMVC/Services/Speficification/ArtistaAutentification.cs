using PruebaMVC.Models;

namespace PruebaMVC.Services.Speficification
{
    public class ArtistaAutentification(int ArtistaId) : IArtistaSpecification
    {
        public bool IsValid(VistaGruposArtista elemento)
        {
            return (elemento.ArtistasId == ArtistaId);
        }
    }
}
