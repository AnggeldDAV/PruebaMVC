using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class Cancione
{
    public int Id { get; set; }

    public string? Titulo { get; set; }

    public string? Genero { get; set; }

    public DateOnly? Fecha { get; set; }

    public int? AlbumesId { get; set; }

    public string? UrlVideo { get; set; }

    public virtual Albume? Albumes { get; set; }

    public virtual ICollection<CancionesConcierto> CancionesConciertos { get; set; } = new List<CancionesConcierto>();

    public virtual ICollection<ListasCancione> ListasCanciones { get; set; } = new List<ListasCancione>();
}
