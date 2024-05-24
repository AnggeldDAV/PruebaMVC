using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(CancioneMetadata))]
    public partial class Cancione { }
    public class CancioneMetadata
    {
        [Required(ErrorMessage = "Introduzca el Id de la cancion")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Introduzca el Titulo de la cancion")]
        public string? Titulo { get; set; }

        public string? Genero { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? Fecha { get; set; }

        [Display(Name = "Album")]
        public int? AlbumesId { get; set; }


        public virtual Albume? Albumes { get; set; }

        public virtual ICollection<CancionesConcierto> CancionesConciertos { get; set; } = new List<CancionesConcierto>();

        public virtual ICollection<ListasCancione> ListasCanciones { get; set; } = new List<ListasCancione>();
    }
}
