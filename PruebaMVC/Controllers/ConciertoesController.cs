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
    public class ConciertoesController : Controller
    {
        private readonly GrupoCContext _context;

        public ConciertoesController(GrupoCContext context)
        {
            _context = context;
        }

        // GET: Conciertoes
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["GeneroSortParm"] = String.IsNullOrEmpty(sortOrder) ? "genero_desc" : "";
            ViewData["LugarSortParm"] = sortOrder == "Lugar" ? "lugar_desc" : "Lugar";
            if (_context.Albumes == null)
            {
                return Problem("Es nulo");
            }
            var conciertos = from m in _context.Conciertos
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                conciertos = conciertos.Where(s => s.Lugar!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "genero_desc":
                    conciertos = conciertos.OrderByDescending(s => s.Genero);
                    break;
                case "Lugar":
                    conciertos = conciertos.OrderBy(s => s.Lugar);
                    break;
                case "lugar_desc":
                    conciertos = conciertos.OrderByDescending(s => s.Lugar);
                    break;
                default:
                    conciertos = conciertos.OrderBy(s => s.Genero);
                    break;
            }
            return View(await conciertos.AsNoTracking().ToListAsync());
        }

        // GET: Conciertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concierto = await _context.Conciertos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concierto == null)
            {
                return NotFound();
            }

            return View(concierto);
        }

        // GET: Conciertoes/Create
        public IActionResult Create()
        {
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero");
            return View();
        }

        // POST: Conciertoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Genero,Lugar")] Concierto concierto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concierto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero", concierto.Genero);
            return View(concierto);
        }

        // GET: Conciertoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concierto = await _context.Conciertos.FindAsync(id);
            if (concierto == null)
            {
                return NotFound();
            }
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero", concierto.Genero);
            return View(concierto);
        }

        // POST: Conciertoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Genero,Lugar")] Concierto concierto)
        {
            if (id != concierto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concierto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConciertoExists(concierto.Id))
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
            ViewData["Genero"] = new SelectList(_context.Albumes, "Id", "Genero", concierto.Genero);
            return View(concierto);
        }

        // GET: Conciertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concierto = await _context.Conciertos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concierto == null)
            {
                return NotFound();
            }

            return View(concierto);
        }

        // POST: Conciertoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concierto = await _context.Conciertos.FindAsync(id);
            if (concierto != null)
            {
                _context.Conciertos.Remove(concierto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConciertoExists(int id)
        {
            return _context.Conciertos.Any(e => e.Id == id);
        }
    }
}
