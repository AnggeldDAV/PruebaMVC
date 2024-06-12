using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class GruposArtistasController : Controller
    {
        private readonly IGenericRepositorio<GruposArtista> _context;
        private readonly IGenericRepositorio<Artista> _contextArtista;
        private readonly IGenericRepositorio<Grupo> _contextGrupo;
        private readonly IGenericRepositorio<VistaGruposArtista> _contextVista;

        public GruposArtistasController(IGenericRepositorio<GruposArtista> context, IGenericRepositorio<Artista> contextArtista, IGenericRepositorio<Grupo> contextGrupo, IGenericRepositorio<VistaGruposArtista> contextVista)
        {
            _context = context;
            _contextArtista = contextArtista;
            _contextGrupo = contextGrupo;
            _contextVista = contextVista;

        }

        // GET: GruposArtistas
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await _contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: GruposArtistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var gruposArtista =vista.FirstOrDefault(m => m.Id == id);
            if (gruposArtista == null)
            {
                return NotFound();
            }

            return View(gruposArtista);
        }

        // GET: GruposArtistas/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ArtistasId"] = new SelectList(await _contextArtista.DameTodos(), "Id", "Nombre");
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre");
            return View();
        }

        // POST: GruposArtistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistasId,GruposId")] GruposArtista gruposArtista)
        {
            if (ModelState.IsValid)
            {
               await _context.Agregar(gruposArtista);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistasId"] = new SelectList(await _contextArtista.DameTodos(), "Id", "Nombre", gruposArtista.ArtistasId);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", gruposArtista.GruposId);
            return View(gruposArtista);
        }

        // GET: GruposArtistas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gruposArtista = await _context.DameUno(id);
            if (gruposArtista == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            ViewData["ArtistasId"] = new SelectList(await _contextArtista.DameTodos(), "Id", "Nombre", gruposArtista.ArtistasId);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", gruposArtista.GruposId);
            return View(conjunto);
        }

        // POST: GruposArtistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistasId,GruposId")] GruposArtista gruposArtista)
        {
            if (id != gruposArtista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Modificar(id,gruposArtista);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GruposArtistaExists(gruposArtista.Id).Result)
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
            ViewData["ArtistasId"] = new SelectList(await _contextArtista.DameTodos(), "Id", "Nombre", gruposArtista.ArtistasId);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", gruposArtista.GruposId);
            return View(conjunto);
        }

        // GET: GruposArtistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var gruposArtista = vista.FirstOrDefault(m => m.Id == id);
            if (gruposArtista == null)
            {
                return NotFound();
            }

            return View(gruposArtista);
        }

        // POST: GruposArtistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gruposArtista = await _context.DameUno(id);
            if (gruposArtista != null)
            {
               await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GruposArtistaExists(int id)
        {
            var vista =await _contextArtista.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
