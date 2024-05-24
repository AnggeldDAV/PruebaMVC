using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;

namespace PruebaMVC.Controllers
{
    public class CancionesController : Controller
    {
        private readonly GrupoCContext _context;

        public CancionesController(GrupoCContext context)
        {
            _context = context;
        }

        // GET: Canciones
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var grupoCContext = _context.Canciones.Include(c => c.Albumes);
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
                    canciones = canciones.OrderBy(s => s.Albumes);
                    break;
                case "albumes_desc":
                    canciones = canciones.OrderByDescending(s => s.Albumes);
                    break;
                default:
                    canciones = canciones.OrderBy(s => s.Titulo);
                    break;
            }
            return View(await canciones.AsNoTracking().ToListAsync());
            
        }

        // GET: Canciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancione = await _context.Canciones
                .Include(c => c.Albumes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }

            return View(cancione);
        }

        // GET: Canciones/Create
        public IActionResult Create()
        {
            ViewData["AlbumesId"] = new SelectList(_context.Albumes, "Id", "Titulo");
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero");
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
                _context.Add(cancione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumesId"] = new SelectList(_context.Albumes, "Id", "Id", cancione.AlbumesId);
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero", cancione.Genero);            
            return View(cancione);
        }

        // GET: Canciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancione = await _context.Canciones.FindAsync(id);
            if (cancione == null)
            {
                return NotFound();
            }
            ViewData["AlbumesId"] = new SelectList(_context.Albumes, "Id", "Id", cancione.AlbumesId);
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero", cancione.Genero);
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
                    _context.Update(cancione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancioneExists(cancione.Id))
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
            ViewData["AlbumesId"] = new SelectList(_context.Albumes, "Id", "Id", cancione.AlbumesId);
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero", cancione.Genero);
            return View(cancione);
        }

        // GET: Canciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancione = await _context.Canciones
                .Include(c => c.Albumes)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var cancione = await _context.Canciones.FindAsync(id);
            if (cancione != null)
            {
                _context.Canciones.Remove(cancione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CancioneExists(int id)
        {
            return _context.Canciones.Any(e => e.Id == id);
        }
    }
}
