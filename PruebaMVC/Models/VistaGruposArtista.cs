using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaGruposArtista
{
    public int Id { get; set; }

    public int? ArtistasId { get; set; }

    public int? GruposId { get; set; }

    public string? NombreGrupo { get; set; }

    public string? NombreArtista { get; set; }

    public string? Genero { get; set; }

    public DateOnly? FechaNac { get; set; }

    public int IdGrupo { get; set; }

    public int IdArtista { get; set; }

    public byte[]? Foto { get; set; }
}
