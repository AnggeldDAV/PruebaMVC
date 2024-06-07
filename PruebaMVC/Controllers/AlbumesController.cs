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

        public AlbumesController(IGenericRepositorio<Albume> context, IGenericRepositorio<Grupo> contextGrupo)
        {
            _context = context;
            _contextGrupo = contextGrupo;
        }

        // GET: Albumes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["IDSortParm"] = sortOrder == "Grupos" ? "grupos_desc" : "Grupos";
            if (_context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }

            var grupos = _contextGrupo.DameTodos().Select(x => x);
            var albums = _context.DameTodos().Select(x => x);
            var conjunto = albums
                           .Join(grupos,
                           album => album.Id,
                           grupo => grupo.Id,
                           (album, grupo) => new VistaAlbume { 
                               Id = album.Id,
                               Titulo = album.Titulo,
                               Fecha= album.Fecha, 
                               Genero=album.Genero,
                               NombreGrupo =grupo.Nombre, 
                               GruposId=grupo.Id, });
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

            var grupos = _contextGrupo.DameTodos().Select(x => x);
            var albums = _context.DameTodos().Select(x => x);
            var conjunto = albums
                           .Join(grupos,
                           album => album.Id,
                           grupo => grupo.Id,
                           (album, grupo) => new VistaAlbume
                           {
                               Id = album.Id,
                               Titulo = album.Titulo,
                               Fecha = album.Fecha,
                               Genero = album.Genero,
                               NombreGrupo = grupo.Nombre,
                               GruposId = grupo.Id,
                           }).Select(x => x).
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
            var grupos = _contextGrupo.DameTodos().Select(x => x);
            var albums = _context.DameTodos().Select(x => x);
            var conjunto = albums
                           .Join(grupos,
                           album => album.Id,
                           grupo => grupo.Id,
                           (album, grupo) => new VistaAlbume
                           {
                               Id = album.Id,
                               Titulo = album.Titulo,
                               Fecha = album.Fecha,
                               Genero = album.Genero,
                               NombreGrupo = grupo.Nombre,
                               GruposId = grupo.Id,
                           });
            var albume = conjunto.FirstOrDefault(x => x.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // GET: Albumes/Create
        public IActionResult Create()
        {
            ViewData["GruposId"] = new SelectList(_contextGrupo.DameTodos(), "Id", "Nombre");
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
            ViewData["GruposId"] = new SelectList(_contextGrupo.DameTodos(), "Id", "Nombre", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = _context.DameUno(id);
            if (albume == null)
            {
                return NotFound();
            }
            ViewData["GruposId"] = new SelectList(_contextGrupo.DameTodos(), "Id", "Nombre", albume.GruposId);
            return View(albume);
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
                    if (!AlbumeExists(albume.Id))
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
            ViewData["GruposId"] = new SelectList(_contextGrupo.DameTodos(), "Id", "Id", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = _context.DameTodos().FirstOrDefault(x => x.Id == id);
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
            var albume = _context.DameUno(id);
            if (albume != null)
            {
                _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumeExists(int id)
        {
            return _context.DameTodos().Any(e => e.Id == id);
        }
    }
}
