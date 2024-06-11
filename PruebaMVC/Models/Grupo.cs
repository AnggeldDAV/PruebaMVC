using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class Grupo
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Albume> Albumes { get; set; } = new List<Albume>();

    public virtual ICollection<ConciertosGrupo> ConciertosGrupos { get; set; } = new List<ConciertosGrupo>();

    public virtual ICollection<GruposArtista> GruposArtista { get; set; } = new List<GruposArtista>();
}
