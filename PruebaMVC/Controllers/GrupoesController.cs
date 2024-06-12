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
    public class GrupoesController : Controller
    {
        private readonly IGenericRepositorio<Grupo> _context;

        public GrupoesController(IGenericRepositorio<Grupo> context)
        {
            _context = context;
        }

        // GET: Grupoes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            if (await _context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }
            var vista = await _context.DameTodos();
            var grupos = vista.Select(x=>x);
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(s => s.Nombre!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "nombre_desc":
                    grupos = grupos.OrderByDescending(s => s.Nombre);
                    break;
                default:
                    grupos = grupos.OrderBy(s => s.Nombre);
                    break;
            }
            return View(grupos);
        }

        public async Task<IActionResult> IndexConArtistas(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            if (await _context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }

            var vista = await _context.DameTodos();
            var grupos = vista.Select(x => x);
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(s => s.Nombre!.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "nombre_desc":
                    grupos = grupos.OrderByDescending(s => s.Nombre);
                    break;
                default:
                    grupos = grupos.OrderBy(s => s.Nombre);
                    break;
            }

            return View(grupos);
        }

        // GET: Grupoes/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _context.DameTodos();
            var grupo = vista
                .FirstOrDefault(m => m.Id == id);
            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo);
        }

        // GET: Grupoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grupoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                await _context.Agregar(grupo);
                return RedirectToAction(nameof(Index));
            }
            return View(grupo);
        }

        // GET: Grupoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupo = await _context.DameUno(id);
            if (grupo == null)
            {
                return NotFound();
            }
            return View(grupo);
        }

        // POST: Grupoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Grupo grupo)
        {
            if (id != grupo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Modificar(id,grupo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoExists(grupo.Id).Result)
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
            return View(grupo);
        }

        // GET: Grupoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _context.DameTodos();
            var grupo = vista
                .FirstOrDefault(m => m.Id == id);
            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo);
        }

        // POST: Grupoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await _context.DameUno(id);
            if (grupo != null)
            {
                await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GrupoExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
