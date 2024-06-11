using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class Concierto
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Genero { get; set; }

    public string? Lugar { get; set; }

    public string? Titulo { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<CancionesConcierto> CancionesConciertos { get; set; } = new List<CancionesConcierto>();

    public virtual ICollection<ConciertosGrupo> ConciertosGrupos { get; set; } = new List<ConciertosGrupo>();
}
