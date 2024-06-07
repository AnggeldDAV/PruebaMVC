using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaCancione
{
    public int IdAlbum { get; set; }

    public DateOnly? FechaAlbumes { get; set; }

    public string? GeneroAlbum { get; set; }

    public string? TituloAlbum { get; set; }

    public int Id { get; set; }

    public string? Titulo { get; set; }

    public string? Genero { get; set; }

    public DateOnly? Fecha { get; set; }

    public int? AlbumesId { get; set; }

    public int? GruposId { get; set; }
}
