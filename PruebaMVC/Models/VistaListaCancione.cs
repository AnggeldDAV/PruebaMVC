using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaListaCancione
{
    public int? CancionesId { get; set; }

    public int? ListasId { get; set; }

    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? UsuarioId { get; set; }

    public string? Titulo { get; set; }

    public string? Genero { get; set; }

    public DateOnly? Fecha { get; set; }

    public int IdListas { get; set; }

    public int IdCanciones { get; set; }

    public string? UrlVideo { get; set; }
}
