using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models;

public partial class Cancione
{
    public int Id { get; set; }
    [Required]
    public string? Titulo { get; set; }

    public string? Genero { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? Fecha { get; set; }

    [Required]
    [Display(Name = "Album")]
    public int? AlbumesId { get; set; }
    

    public virtual Albume? Albumes { get; set; }

    public virtual ICollection<CancionesConcierto> CancionesConciertos { get; set; } = new List<CancionesConcierto>();

    public virtual ICollection<ListasCancione> ListasCanciones { get; set; } = new List<ListasCancione>();
}
