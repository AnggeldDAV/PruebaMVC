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
    public class AlbumesController : Controller
    {
        private readonly GrupoCContext _context;

        public AlbumesController(GrupoCContext context)
        {
            _context = context;
        }

        // GET: Albumes
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["IDSortParm"] = sortOrder == "Grupos" ? "grupos_desc" : "Grupos";
            if (_context.Albumes == null)
            {
                return Problem("Es nulo");
            }
            var albums = from m in _context.Albumes
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(s => s.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    albums = albums.OrderByDescending(s => s.Titulo);
                    break;
                case "Genero":
                    albums = albums.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    albums = albums.OrderByDescending(s => s.Genero);
                    break;
                    case "Grupos":
                    albums = albums.OrderBy(s => s.GruposId);
                    break;                    
                    case "grupos_desc":
                    albums = albums.OrderByDescending(s => s.GruposId);
                    break;
                    default:
                    albums = albums.OrderBy(s => s.Titulo);
                    break;
            }
            return View(await albums.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> IndexConsulta()
        {
            var consulta = _context.Albumes.Where(x=>x.Genero == "Heavy Metal" && x.Titulo.Contains("u"));
            return View(await consulta.AsNoTracking().ToListAsync());
        }

        // GET: Albumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = await _context.Albumes
                .Include(a => a.Grupos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // GET: Albumes/Create
        public IActionResult Create()
        {
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Nombre");
            return View();
        }

        // POST: Albumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Genero,Titulo,GruposId")] Albume albume)
        {
            if (ModelState.IsValid)
            {
                _context.Add(albume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Nombre", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = await _context.Albumes.FindAsync(id);
            if (albume == null)
            {
                return NotFound();
            }
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Nombre", albume.GruposId);
            return View(albume);
        }

        // POST: Albumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Genero,Titulo,GruposId")] Albume albume)
        {
            if (id != albume.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(albume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumeExists(albume.Id))
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
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Id", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = await _context.Albumes
                .Include(a => a.Grupos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // POST: Albumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albume = await _context.Albumes.FindAsync(id);
            if (albume != null)
            {
                _context.Albumes.Remove(albume);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumeExists(int id)
        {
            return _context.Albumes.Any(e => e.Id == id);
        }
    }
}
