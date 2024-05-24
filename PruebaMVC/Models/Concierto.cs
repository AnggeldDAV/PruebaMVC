using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models;

public partial class Concierto
{
    public int Id { get; set; }
    [DataType(DataType.DateTime)]
    [Required]
    public DateTime? Fecha { get; set; }

    public string? Genero { get; set; }
    [Required]
    public string? Lugar { get; set; }

    public virtual ICollection<CancionesConcierto> CancionesConciertos { get; set; } = new List<CancionesConcierto>();

    public virtual ICollection<ConciertosGrupo> ConciertosGrupos { get; set; } = new List<ConciertosGrupo>();
}
