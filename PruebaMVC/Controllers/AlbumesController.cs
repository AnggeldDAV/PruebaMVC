using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class AlbumesController : Controller
    {
        private readonly IGenericRepositorio<Albume> _context;
        private readonly IGenericRepositorio<Grupo> _contextGrupo;
        private readonly IGenericRepositorio<VistaAlbume> _contextVista;


        public AlbumesController(IGenericRepositorio<Albume> context, IGenericRepositorio<Grupo> contextGrupo, IGenericRepositorio<VistaAlbume> contextVista)
        {
            _context = context;
            _contextGrupo = contextGrupo;
            _contextVista = contextVista;
        }

        // GET: Albumes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["IDSortParm"] = sortOrder == "Grupos" ? "grupos_desc" : "Grupos";
            if (await _context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.Select(x => x);
            if (!String.IsNullOrEmpty(searchString))
            {
                conjunto = conjunto.Where(s => s.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Titulo);
                    break;
                case "Genero":
                    conjunto =  conjunto.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Genero);
                    break;
                case "Grupos":
                    conjunto = conjunto.OrderBy(s => s.NombreGrupo);
                    break;
                case "grupos_desc":
                    conjunto = conjunto.OrderByDescending(s => s.NombreGrupo);
                    break;
                default:
                    conjunto = conjunto.OrderBy(s => s.Titulo);
                    break;
            }

            return View(conjunto);
        }

        public async Task<IActionResult> IndexConsulta()
        {
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.Select(x => x).
                           Where(x => x.Genero == "Heavy Metal" && x.Titulo.Contains("u"));
            return View(conjunto);
        }

        // GET: Albumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var conjunto = await _contextVista.DameTodos();
            var albume = conjunto.FirstOrDefault(x => x.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // GET: Albumes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre");
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
                _context.Agregar(albume);
                return RedirectToAction(nameof(Index));
            }
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = await _context.DameUno(id);
            if (albume == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", albume.GruposId);
            return View(conjunto);
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
                    _context.Modificar(id, albume);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumeExists(albume.Id).Result)
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
            ViewData["GruposId"] = new SelectList(await _contextGrupo.DameTodos(), "Id", "Nombre", albume.GruposId);
            return View(conjunto);
        }

        // GET: Albumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await _contextVista.DameTodos();
            var albume =vista.FirstOrDefault(x => x.Id == id);
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
            var albume = await _context.DameUno(id);
            if (albume != null)
            {
               await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AlbumeExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
