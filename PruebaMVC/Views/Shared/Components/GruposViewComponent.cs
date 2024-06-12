using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;
using PruebaMVC.Services.Speficification;

namespace PruebaMVC.Views.Shared.Components
{
    public class GruposViewComponent(IGenericRepositorio<VistaGruposArtista> coleccion) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int ArtistaId)
        {
            var items = await coleccion.DameTodos();
            IArtistaSpecification especificacion = new ArtistaAutentification(ArtistaId);
            var itemsFiltrados = items.Where(especificacion.IsValid);
            return View(itemsFiltrados);
        }
    }
}