using System;
using System.Collections.Generic;

namespace PruebaMVC.Models;

public partial class VistaAlbume
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Genero { get; set; }

    public string? Titulo { get; set; }

    public string? NombreGrupo { get; set; }

    public int? GruposId { get; set; }

    public int IdGrupos { get; set; }
}
