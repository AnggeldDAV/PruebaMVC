using PruebaMVC.Models;

namespace PruebaMVC.Services.Speficification
{
    public class GrupoAutentificacion(int GrupoId) : IGruposSpecification
    {
        public bool IsValid(VistaGruposArtista elemento)
        {
            return (elemento.GruposId == GrupoId);
        }
    }
}
