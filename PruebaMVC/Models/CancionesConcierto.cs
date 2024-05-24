using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class CancionesConcierto
{
    public int Id { get; set; }

    public int? CancionesId { get; set; }

    public int? ConciertosId { get; set; }

    public virtual Cancione? Canciones { get; set; }

    public virtual Concierto? Conciertos { get; set; }
}
