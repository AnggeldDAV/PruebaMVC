using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(ConciertoMetadata))]
    public partial class Concierto { }
    public class ConciertoMetadata
    {
        public int Id { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Genero { get; set; }
        //[Required(ErrorMessage = "Introduzca el Lugar del Concierto")]
        public string? Lugar { get; set; }
        //[Required(ErrorMessage = "Introduzca el Titulo del Concierto")]
        public string? Titulo { get; set; }
        //[Required(ErrorMessage = "Introduzca el Precio del Concierto")]
        public decimal? Precio { get; set; }
        public virtual ICollection<CancionesConcierto> CancionesConciertos { get; set; } = new List<CancionesConcierto>();
        public virtual ICollection<ConciertosGrupo> ConciertosGrupos { get; set; } = new List<ConciertosGrupo>();
    }
}
