using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class Artista
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Genero { get; set; }

    public DateOnly? FechaNac { get; set; }

    public byte[]? Foto { get; set; }

    public virtual ICollection<GruposArtista> GruposArtista { get; set; } = new List<GruposArtista>();
}
