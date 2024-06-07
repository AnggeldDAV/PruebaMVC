using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaListum
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? UsuarioId { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Email { get; set; }

    public string? Contraseña { get; set; }

    public int IdUsuario { get; set; }
}
