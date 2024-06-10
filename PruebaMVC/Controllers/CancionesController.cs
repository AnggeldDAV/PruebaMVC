using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class CancionesController : Controller
    {
        private readonly IGenericRepositorio<Cancione> _context;
        private readonly IGenericRepositorio<Albume> _contextAlbume;
        private readonly IGenericRepositorio<VistaCancione> _contextVista;

        public CancionesController(IGenericRepositorio<Cancione> context, IGenericRepositorio<Albume> contextAlbume,IGenericRepositorio<VistaCancione> contextVista)
        {
            _context = context;
            _contextAlbume = contextAlbume;
            _contextVista = contextVista;
        }

        // GET: Canciones
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var grupoCContext = await _contextVista.DameTodos();
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["AlbumesSortParm"] = sortOrder == "Albumes" ? "albumes_desc" : "Albumes";
            if (grupoCContext == null)
            {
                return Problem("Es nulo");
            }
            var canciones = from m in grupoCContext
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                canciones = canciones.Where(s => s.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "titulo_desc":
                    canciones = canciones.OrderByDescending(s => s.Titulo);
                    break;
                case "Genero":
                    canciones = canciones.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    canciones = canciones.OrderByDescending(s => s.Genero);
                    break;                
                case "Albumes":
                    canciones = canciones.OrderBy(s => s.TituloAlbum);
                    break;
                case "albumes_desc":
                    canciones = canciones.OrderByDescending(s => s.TituloAlbum);
                    break;
                default:
                    canciones = canciones.OrderBy(s => s.Titulo);
                    break;
            }
            return View(canciones);
            
        }

        // GET: Canciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var cancione = vista
                .FirstOrDefault(m => m.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }

            return View(cancione);
        }

        // GET: Canciones/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AlbumesId"] = new SelectList(await _contextAlbume.DameTodos(), "Id", "Titulo");
            return View();
        }

        // POST: Canciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Genero,Fecha,AlbumesId")] Cancione cancione)
        {
            if (ModelState.IsValid)
            {
                await _context.Agregar(cancione);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumesId"] = new SelectList(await _contextAlbume.DameTodos(), "Id", "Id", cancione.AlbumesId);          
            return View(cancione);
        }

        // GET: Canciones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var conjunto = await _contextVista.DameTodos();
            var cancione = conjunto.FirstOrDefault(x => x.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }
            ViewData["AlbumesId"] = new SelectList(await _contextAlbume.DameTodos(), "Id", "Titulo", cancione.AlbumesId);
            return View(cancione);
        }

        // POST: Canciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Genero,Fecha,AlbumesId")] Cancione cancione)
        {
            if (id != cancione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Modificar(id,cancione);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancioneExists(cancione.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            ViewData["AlbumesId"] = new SelectList(await _contextAlbume.DameTodos(), "Id", "Id", cancione.AlbumesId);
            return View(conjunto);
        }

        // GET: Canciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var cancione = vista.FirstOrDefault(m => m.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }

            return View(cancione);
        }

        // POST: Canciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cancione = await _context.DameUno(id);
            if (cancione != null)
            {
               await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CancioneExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
