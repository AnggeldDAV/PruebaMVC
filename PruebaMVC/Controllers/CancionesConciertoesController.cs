using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;

namespace PruebaMVC.Controllers
{
    public class CancionesConciertoesController : Controller
    {
        private readonly GrupoCContext _context;

        public CancionesConciertoesController(GrupoCContext context)
        {
            _context = context;
        }

        // GET: CancionesConciertoes
        public async Task<IActionResult> Index()
        {
            var grupoCContext = _context.CancionesConciertos.Include(c => c.Canciones).Include(c => c.Conciertos);
            return View(await grupoCContext.ToListAsync());
        }

        // GET: CancionesConciertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancionesConcierto = await _context.CancionesConciertos
                .Include(c => c.Canciones)
                .Include(c => c.Conciertos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancionesConcierto == null)
            {
                return NotFound();
            }

            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Create
        public IActionResult Create()
        {
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id");
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id");
            return View();
        }

        // POST: CancionesConciertoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CancionesId,ConciertosId")] CancionesConcierto cancionesConcierto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cancionesConcierto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id", cancionesConcierto.CancionesId);
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id", cancionesConcierto.ConciertosId);
            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancionesConcierto = await _context.CancionesConciertos.FindAsync(id);
            if (cancionesConcierto == null)
            {
                return NotFound();
            }
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id", cancionesConcierto.CancionesId);
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id", cancionesConcierto.ConciertosId);
            return View(cancionesConcierto);
        }

        // POST: CancionesConciertoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CancionesId,ConciertosId")] CancionesConcierto cancionesConcierto)
        {
            if (id != cancionesConcierto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cancionesConcierto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancionesConciertoExists(cancionesConcierto.Id))
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
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id", cancionesConcierto.CancionesId);
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id", cancionesConcierto.ConciertosId);
            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancionesConcierto = await _context.CancionesConciertos
                .Include(c => c.Canciones)
                .Include(c => c.Conciertos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancionesConcierto == null)
            {
                return NotFound();
            }

            return View(cancionesConcierto);
        }

        // POST: CancionesConciertoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cancionesConcierto = await _context.CancionesConciertos.FindAsync(id);
            if (cancionesConcierto != null)
            {
                _context.CancionesConciertos.Remove(cancionesConcierto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CancionesConciertoExists(int id)
        {
            return _context.CancionesConciertos.Any(e => e.Id == id);
        }
    }
}
