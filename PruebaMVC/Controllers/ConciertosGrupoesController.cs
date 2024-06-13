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
    public class ConciertosGrupoesController : Controller
    {
        private readonly IGenericRepositorio<ConciertosGrupo> _context;
        private readonly IGenericRepositorio<Concierto> _contextConcierto;
        private readonly IGenericRepositorio<Grupo> _contextGrupo;
        private readonly IGenericRepositorio<VistaConciertosGrupo> _contextVista;


        public ConciertosGrupoesController(IGenericRepositorio<ConciertosGrupo> context, IGenericRepositorio<Concierto> contextConcierto, IGenericRepositorio<Grupo> contextGrupo, IGenericRepositorio<VistaConciertosGrupo> contextVista)
        {
            _context = context;
            _contextConcierto = contextConcierto;
            _contextGrupo = contextGrupo;
            _contextVista = contextVista;
        }

        // GET: ConciertosGrupoes
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await _contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: ConciertosGrupoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var conciertosGrupo = vista.AsParallel().FirstOrDefault(m => m.Id == id);
            if (conciertosGrupo == null)
            {
                return NotFound();
            }

            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo");
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre");
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
                await _context.Agregar(conciertosGrupo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo", conciertosGrupo.ConciertosId);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", conciertosGrupo.GruposId);
            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciertosGrupo = await _context.DameUno(id);
            if (conciertosGrupo == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo", conciertosGrupo.ConciertosId);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", conciertosGrupo.GruposId);
            return View(conjunto);
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
                    await _context.Modificar(id,conciertosGrupo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConciertosGrupoExists(conciertosGrupo.Id).Result)
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
            ViewData["ConciertosId"] = new SelectList(await _contextConcierto.DameTodos(), "Id", "Titulo", conciertosGrupo.ConciertosId);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", conciertosGrupo.GruposId);
            return View(conjunto);
        }

        // GET: ConciertosGrupoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var conciertosGrupo = vista.AsParallel()
                .FirstOrDefault(m => m.Id == id);
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
            var conciertosGrupo = await _context.DameUno(id);
            if (conciertosGrupo != null)
            {
                await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ConciertosGrupoExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.AsParallel().Any(e => e.Id == id);
        }
    }
}
