using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaCancionConcierto
{
    public int Id { get; set; }

    public int? CancionesId { get; set; }

    public int? ConciertosId { get; set; }

    public string? Titulo { get; set; }

    public string? TituloCanciones { get; set; }

    public string? GeneroCanciones { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Lugar { get; set; }

    public string? GeneroConcierto { get; set; }

    public decimal? Precio { get; set; }

    public int Expr3 { get; set; }

    public int? AlbumesId { get; set; }

    public DateTime? FechaConcierto { get; set; }

    public int IdCanciones { get; set; }
}
