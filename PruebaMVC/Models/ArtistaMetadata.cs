using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(ArtistaMetadata))]
    public partial class Artista { }
    public class ArtistaMetadata
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Introduzca el Nombre del Artistas")]
        public string? Nombre { get; set; }

        public string? Genero { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateOnly? FechaNac { get; set; }

        public virtual ICollection<GruposArtista> GruposArtista { get; set; } = new List<GruposArtista>();
    }
}
