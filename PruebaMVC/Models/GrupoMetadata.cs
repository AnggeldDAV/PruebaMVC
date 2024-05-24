using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(GrupoMetadata))]
    public partial class Grupo { }
    public class GrupoMetadata
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Introduzca el Nombre del Grupo")]
        public string? Nombre { get; set; }

        public virtual ICollection<Albume> Albumes { get; set; } = new List<Albume>();

        public virtual ICollection<ConciertosGrupo> ConciertosGrupos { get; set; } = new List<ConciertosGrupo>();

        public virtual ICollection<GruposArtista> GruposArtista { get; set; } = new List<GruposArtista>();
    }
}
