using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models;

public partial class Artista
{
    public int Id { get; set; }
    [Required]
    public string? Nombre { get; set; }

    public string? Genero { get; set; }

    [Display(Name = "Fecha de Nacimiento")]
    [DataType(DataType.Date)]
    public DateOnly? FechaNac { get; set; }

    public virtual ICollection<GruposArtista> GruposArtista { get; set; } = new List<GruposArtista>();
}
