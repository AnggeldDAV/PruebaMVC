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
    public class ConciertosGrupoesController : Controller
    {
        private readonly GrupoCContext _context;

        public ConciertosGrupoesController(GrupoCContext context)
        {
            _context = context;
        }

        // GET: ConciertosGrupoes
        public async Task<IActionResult> Index()
        {
            var grupoCContext = _context.ConciertosGrupos.Include(c => c.Conciertos).Include(c => c.Grupos);
            return View(await grupoCContext.ToListAsync());
        }

        // GET: ConciertosGrupoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciertosGrupo = await _context.ConciertosGrupos
                .Include(c => c.Conciertos)
                .Include(c => c.Grupos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conciertosGrupo == null)
            {
                return NotFound();
            }

            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Create
        public IActionResult Create()
        {
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id");
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Id");
            return View();
        }

        // POST: ConciertosGrupoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GruposId,ConciertosId")] ConciertosGrupo conciertosGrupo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conciertosGrupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id", conciertosGrupo.ConciertosId);
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Id", conciertosGrupo.GruposId);
            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciertosGrupo = await _context.ConciertosGrupos.FindAsync(id);
            if (conciertosGrupo == null)
            {
                return NotFound();
            }
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id", conciertosGrupo.ConciertosId);
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Id", conciertosGrupo.GruposId);
            return View(conciertosGrupo);
        }

        // POST: ConciertosGrupoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GruposId,ConciertosId")] ConciertosGrupo conciertosGrupo)
        {
            if (id != conciertosGrupo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conciertosGrupo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConciertosGrupoExists(conciertosGrupo.Id))
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
            ViewData["ConciertosId"] = new SelectList(_context.Conciertos, "Id", "Id", conciertosGrupo.ConciertosId);
            ViewData["GruposId"] = new SelectList(_context.Grupos, "Id", "Id", conciertosGrupo.GruposId);
            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciertosGrupo = await _context.ConciertosGrupos
                .Include(c => c.Conciertos)
                .Include(c => c.Grupos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conciertosGrupo == null)
            {
                return NotFound();
            }

            return View(conciertosGrupo);
        }

        // POST: ConciertosGrupoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conciertosGrupo = await _context.ConciertosGrupos.FindAsync(id);
            if (conciertosGrupo != null)
            {
                _context.ConciertosGrupos.Remove(conciertosGrupo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConciertosGrupoExists(int id)
        {
            return _context.ConciertosGrupos.Any(e => e.Id == id);
        }
    }
}
