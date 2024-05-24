using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class ConciertosGrupo
{
    public int Id { get; set; }

    public int? GruposId { get; set; }

    public int? ConciertosId { get; set; }

    public virtual Concierto? Conciertos { get; set; }

    public virtual Grupo? Grupos { get; set; }
}
