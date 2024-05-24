using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(AlbumeMetadata))]
    public partial class Albume { }
    public class AlbumeMetadata
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? Fecha { get; set; }
        public string? Genero { get; set; }
        [Required(ErrorMessage ="Introduzca un Titulo al Album")]
        public string? Titulo { get; set; }

        [Display(Name = "Grupos")]
        public int? GruposId { get; set; }

        public virtual ICollection<Cancione> Canciones { get; set; } = new List<Cancione>();

        public virtual Grupo? Grupos { get; set; }
    }
}
