using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Views.Shared.Components
{
    public class GrupoViewComponent(IGenericRepositorio<VistaGruposArtista> coleccion) :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int AutorId)
        {
            //var items = await coleccion.DameTodos();
            //ILibroSpecification especificacion = new AutorSpecification(AutorId);
            //var itemsFiltrados = items.Where(especificacion.IsValid);
            return View(AutorId);
        }
    }
}