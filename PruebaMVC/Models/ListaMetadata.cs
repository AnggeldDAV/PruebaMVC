using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaMVC.Models
{
    [ModelMetadataType(typeof(ListaMetadata))]
    public partial class Lista { }
    public class ListaMetadata
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Introduzca el Nombre de la lista")]
        public string? Nombre { get; set; }

        public int? UsuarioId { get; set; }

        public virtual ICollection<ListasCancione> ListasCanciones { get; set; } = new List<ListasCancione>();

        public virtual Usuario? Usuario { get; set; }
    }
}
