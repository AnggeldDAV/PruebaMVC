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
    public class ListasCancionesController : Controller
    {
        private readonly GrupoCContext _context;

        public ListasCancionesController(GrupoCContext context)
        {
            _context = context;
        }

        // GET: ListasCanciones
        public async Task<IActionResult> Index()
        {
            var grupoCContext = _context.ListasCanciones.Include(l => l.Canciones).Include(l => l.Listas);
            return View(await grupoCContext.ToListAsync());
        }

        // GET: ListasCanciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listasCancione = await _context.ListasCanciones
                .Include(l => l.Canciones)
                .Include(l => l.Listas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listasCancione == null)
            {
                return NotFound();
            }

            return View(listasCancione);
        }

        // GET: ListasCanciones/Create
        public IActionResult Create()
        {
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id");
            ViewData["ListasId"] = new SelectList(_context.Listas, "Id", "Id");
            return View();
        }

        // POST: ListasCanciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ListasId,CancionesId")] ListasCancione listasCancione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listasCancione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id", listasCancione.CancionesId);
            ViewData["ListasId"] = new SelectList(_context.Listas, "Id", "Id", listasCancione.ListasId);
            return View(listasCancione);
        }

        // GET: ListasCanciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listasCancione = await _context.ListasCanciones.FindAsync(id);
            if (listasCancione == null)
            {
                return NotFound();
            }
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id", listasCancione.CancionesId);
            ViewData["ListasId"] = new SelectList(_context.Listas, "Id", "Id", listasCancione.ListasId);
            return View(listasCancione);
        }

        // POST: ListasCanciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ListasId,CancionesId")] ListasCancione listasCancione)
        {
            if (id != listasCancione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listasCancione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListasCancioneExists(listasCancione.Id))
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
            ViewData["CancionesId"] = new SelectList(_context.Canciones, "Id", "Id", listasCancione.CancionesId);
            ViewData["ListasId"] = new SelectList(_context.Listas, "Id", "Id", listasCancione.ListasId);
            return View(listasCancione);
        }

        // GET: ListasCanciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listasCancione = await _context.ListasCanciones
                .Include(l => l.Canciones)
                .Include(l => l.Listas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listasCancione == null)
            {
                return NotFound();
            }

            return View(listasCancione);
        }

        // POST: ListasCanciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listasCancione = await _context.ListasCanciones.FindAsync(id);
            if (listasCancione != null)
            {
                _context.ListasCanciones.Remove(listasCancione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListasCancioneExists(int id)
        {
            return _context.ListasCanciones.Any(e => e.Id == id);
        }
    }
}
