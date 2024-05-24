﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models;

public partial class Lista
{
    public int Id { get; set; }
    [Required]
    public string? Nombre { get; set; }

    public int? UsuarioId { get; set; }

    public virtual ICollection<ListasCancione> ListasCanciones { get; set; } = new List<ListasCancione>();

    public virtual Usuario? Usuario { get; set; }
}
