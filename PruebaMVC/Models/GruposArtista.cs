using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class GruposArtista
{
    public int Id { get; set; }

    public int? ArtistasId { get; set; }

    public int? GruposId { get; set; }

    public virtual Artista? Artistas { get; set; }

    public virtual Grupo? Grupos { get; set; }
}
