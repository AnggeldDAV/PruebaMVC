using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario { }
    public class UsuarioMetadata
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Introduzca el Nombre de usuario")]
        public string? Nombre { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Introduzca el Correo Electronico")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Introduce la Contraseña")]
        public string? Contraseña { get; set; }

        public virtual ICollection<Lista> Lista { get; set; } = new List<Lista>();
    }
}
