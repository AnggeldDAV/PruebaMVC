using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class CancionesConciertoesController : Controller
    {
        private readonly IGenericRepositorio<CancionesConcierto> _context;
        private readonly IGenericRepositorio<Cancione> _contextCancione;
        private readonly IGenericRepositorio<Concierto> _contextConcierto;
        private readonly IGenericRepositorio<VistaCancionConcierto> _contextVista;


        public CancionesConciertoesController(IGenericRepositorio<CancionesConcierto> context, IGenericRepositorio<Cancione> contextCancione, IGenericRepositorio<Concierto> contextConcierto, IGenericRepositorio<VistaCancionConcierto> contextVista)
        {
            _context = context;
            _contextCancione = contextCancione;
            _contextConcierto = contextConcierto;
            _contextVista = contextVista;
        }

        // GET: CancionesConciertoes
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await _contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: CancionesConciertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var cancionesConcierto = vista.AsParallel().FirstOrDefault(m => m.Id == id);
            if (cancionesConcierto == null)
            {
                return NotFound();
            }

            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CancionesId"] = new SelectList(await _contextCancione.DameTodos(), "Id", "Titulo");
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo");
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
                await _context.Agregar(cancionesConcierto);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CancionesId"] = new SelectList(await _contextCancione.DameTodos(), "Id", "Titulo", cancionesConcierto.CancionesId);
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo", cancionesConcierto.ConciertosId);
            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancionesConcierto = await _context.DameUno(id);
            if (cancionesConcierto == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            ViewData["CancionesId"] = new SelectList(await _contextCancione.DameTodos(), "Id", "Titulo", cancionesConcierto.CancionesId);
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo", cancionesConcierto.ConciertosId);
            return View(conjunto);
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
                    await _context.Modificar(id,cancionesConcierto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancionesConciertoExists(cancionesConcierto.Id).Result)
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
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            ViewData["CancionesId"] = new SelectList(await _contextCancione.DameTodos(), "Id", "Titulo", cancionesConcierto.CancionesId);
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo", cancionesConcierto.ConciertosId);
            return View(conjunto);
        }

        // GET: CancionesConciertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var cancionesConcierto = vista.AsParallel().FirstOrDefault(m => m.Id == id);
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
            var cancionesConcierto = await _context.DameUno(id);
            if (cancionesConcierto != null)
            {
                await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CancionesConciertoExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.AsParallel().Any(e => e.Id == id);
        }
    }
}
