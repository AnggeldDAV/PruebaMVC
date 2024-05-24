using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }
    [DataType(DataType.EmailAddress)]
    [Required]
    public string? Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string? Contraseña { get; set; }

    public virtual ICollection<Lista> Lista { get; set; } = new List<Lista>();
}
