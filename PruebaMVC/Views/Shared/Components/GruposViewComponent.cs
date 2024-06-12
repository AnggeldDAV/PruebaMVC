using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;
using PruebaMVC.Services.Speficification;

namespace PruebaMVC.Views.Shared.Components
{
    public class GruposViewComponent(IGenericRepositorio<VistaGruposArtista> coleccion) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IGruposSpecification especificacion)
        {
            var items = await coleccion.DameTodos();
            var itemsFiltrados = items.Where(especificacion.IsValid);
            return View(itemsFiltrados);
        }
    }
}