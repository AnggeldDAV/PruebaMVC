using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class Lista
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? UsuarioId { get; set; }

    public virtual ICollection<ListasCancione> ListasCanciones { get; set; } = new List<ListasCancione>();

    public virtual Usuario? Usuario { get; set; }
}
