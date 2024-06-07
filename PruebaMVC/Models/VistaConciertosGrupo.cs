using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaConciertosGrupo
{
    public string? Nombre { get; set; }

    public int Id { get; set; }

    public int? GruposId { get; set; }

    public int? ConciertosId { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Genero { get; set; }

    public string? Lugar { get; set; }

    public string? Titulo { get; set; }

    public decimal? Precio { get; set; }

    public int IdConcierto { get; set; }

    public int IdGrupos { get; set; }
}
