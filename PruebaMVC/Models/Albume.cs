using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models;

public partial class Albume
{
    public int Id { get; set; }
    [DataType(DataType.Date)]
    public DateOnly? Fecha { get; set; }
    public string? Genero { get; set; }
    public string? Titulo { get; set; }

    public int? GruposId { get; set; }

    public virtual ICollection<Cancione> Canciones { get; set; } = new List<Cancione>();

    public virtual Grupo? Grupos { get; set; }
}
