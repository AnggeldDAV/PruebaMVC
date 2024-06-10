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
    public class ConciertoesController : Controller
    {
        private readonly IGenericRepositorio<Concierto> _context;

        public ConciertoesController(IGenericRepositorio<Concierto> context)
        {
            _context = context;
        }

        // GET: Conciertoes
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["LugarSortParm"] = sortOrder == "Lugar" ? "lugar_desc" : "Lugar";
            ViewData["PrecioSortParm"] = sortOrder == "Precio" ? "precio_desc" : "Precio";
            if (await _context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }

            var vista = await _context.DameTodos();
            var conciertos = vista.Select(x=>x);

            if (!String.IsNullOrEmpty(searchString))
            {
                conciertos = conciertos.Where(s => s.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "titulo_desc":
                    conciertos = conciertos.OrderByDescending(s => s.Titulo);
                    break;
                case "Lugar":
                    conciertos = conciertos.OrderBy(s => s.Lugar);
                    break;
                case "lugar_desc":
                    conciertos = conciertos.OrderByDescending(s => s.Lugar);
                    break;
                case "Genero":
                    conciertos = conciertos.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    conciertos = conciertos.OrderByDescending(s => s.Genero);
                    break;
                case "Precio":
                    conciertos = conciertos.OrderBy(s => s.Precio);
                    break;
                case "precio_desc":
                    conciertos = conciertos.OrderByDescending(s => s.Precio);
                    break;
                default:
                    conciertos = conciertos.OrderBy(s => s.Titulo);
                    break;
            }
            return View(conciertos);
        }

        public async Task<IActionResult> IndexConsulta()
        {
            var vista = await _context.DameTodos();
            var consulta = vista.Where(x=> x.Fecha.Value.Year >2015 && x.Precio >30);
            return View(consulta);
        }

        // GET: Conciertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_context.DameUno((int)id) == null)
            {
                return NotFound();
            }

            var vista = await _context.DameTodos();
            var context = vista.Select(x=>x);
            var concierto = context
                .FirstOrDefault(m => m.Id == id);
            if (concierto == null)
            {
                return NotFound();
            }

            return View(concierto);
        }

        // GET: Conciertoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conciertoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Id,Fecha,Genero,Lugar,Precio")] Concierto concierto)
        {
            if (ModelState.IsValid)
            {
                await _context.Agregar(concierto);
                return RedirectToAction(nameof(Index));
            }
            return View(concierto);
        }

        // GET: Conciertoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concierto = await _context.DameUno((int)id);
            if (concierto == null)
            {
                return NotFound();
            }
            return View(concierto);
        }

        // POST: Conciertoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Titulo,Id,Fecha,Genero,Lugar,Precio")] Concierto concierto)
        {
            if (id != concierto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   _context.Modificar(id,concierto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConciertoExists(concierto.Id).Result)
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
            return View(concierto);
        }

        // GET: Conciertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _context.DameTodos();
            var context = vista.Select(x => x);
            var concierto = context
                .FirstOrDefault(m => m.Id == id);
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
            var concierto = await _context.DameUno((int)id);
            if (concierto != null)
            {
                await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ConciertoExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
