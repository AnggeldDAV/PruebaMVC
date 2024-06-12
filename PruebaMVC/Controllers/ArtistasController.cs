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
    public class ArtistasController : Controller
    {
        private readonly IGenericRepositorio<Artista> _context;


        public ArtistasController(IGenericRepositorio<Artista> context)
        {
            _context = context;
        }

        // GET: Artistas
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            if (await _context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }

            var vista = await _context.DameTodos();
            var artistas = from m in vista
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                artistas = artistas.Where(s => s.Nombre!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "nombre_desc":
                    artistas = artistas.OrderByDescending(s => s.Nombre);
                    break;
                case "Genero":
                    artistas = artistas.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    artistas = artistas.OrderByDescending(s => s.Genero);
                    break;
                default:
                    artistas = artistas.OrderBy(s => s.Nombre);
                    break;
            }
            return View(artistas);
        }

        public async Task<IActionResult> IndexConsulta()
        {
            var vista = await _context.DameTodos();
            var consulta = vista.Where(x => x.FechaNac.Value.Year >1950);
            return View(consulta);
        }

        // GET: Artistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _context.DameTodos();
            var artista = vista
                .FirstOrDefault(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        // GET: Artistas/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Artistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Genero,FechaNac")] Artista artista, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await foto.CopyToAsync(memoryStream);
                        artista.Foto = memoryStream.ToArray();
                    }
                }
                await _context.Agregar(artista);
                return RedirectToAction(nameof(Index));
            }
            return View(artista);
        }

        // GET: Artistas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artista = await _context.DameUno(id);
            if (artista == null)
            {
                return NotFound();
            }
            return View(artista);
        }

        // POST: Artistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Genero,FechaNac")] Artista artista, IFormFile foto)
        {
            if (id != artista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var artistaToUpdate = await _context.DameUno(id);
                    if (foto != null && foto.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await foto.CopyToAsync(memoryStream);
                            artistaToUpdate.Foto = memoryStream.ToArray();
                        }
                    }

                    artistaToUpdate.Nombre = artista.Nombre;
                    artistaToUpdate.Genero = artista.Genero;
                    artistaToUpdate.FechaNac = artista.FechaNac;
                    await _context.Modificar(id, artistaToUpdate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistaExists(artista.Id).Result)
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
            return View(artista);
        }

        // GET: Artistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _context.DameTodos();
            var artista = vista
                .FirstOrDefault(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        // POST: Artistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artista = await _context.DameUno(id);
            if (artista != null)
            {
               await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ArtistaExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
