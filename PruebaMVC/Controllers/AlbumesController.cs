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
    public class AlbumesController : Controller
    {
        private readonly IGenericRepositorio<Albume> _context;
        private readonly IGenericRepositorio<Grupo> _Grupocontext;

        public AlbumesController(IGenericRepositorio<Albume> context, IGenericRepositorio<Grupo> grupocontext)
        {
            _context = context;
            _Grupocontext = grupocontext;
        }

        // GET: Albumes
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["IDSortParm"] = sortOrder == "Grupos" ? "grupos_desc" : "Grupos";
            if (_context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }

            var albums = _context.DameTodos().Join(_Grupocontext.DameTodos(), album => album.GruposId,
                grupo => grupo.Id, (album, grupo) => new { Album = album, Grupo = grupo }).Select(x=>x);
             
            if (!String.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(s => s.Album.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    albums = albums.OrderByDescending(s => s.Album.Titulo);
                    break;
                case "Genero":
                    albums = albums.OrderBy(s => s.Album.Genero);
                    break;
                case "genero_desc":
                    albums = albums.OrderByDescending(s => s.Album.Genero);
                    break;
                    case "Grupos":
                    albums = albums.OrderBy(s => s.Grupo.Nombre);
                    break;                    
                    case "grupos_desc":
                    albums = albums.OrderByDescending(s => s.Grupo.Nombre);
                    break;
                    default:
                    albums = albums.OrderBy(s => s.Album.Titulo);
                    break;
            }
            return View(albums);
        }

        public async Task<IActionResult> IndexConsulta()
        {
            var consulta = _context.DameTodos().Join(_Grupocontext.DameTodos(), album => album.GruposId,
                grupo => grupo.Id, (album, grupo) => new { Album = album, Grupo = grupo }).Select(x => x).Select(x => x).Where(x=>x.Album.Genero == "Heavy Metal" && x.Album.Titulo.Contains("u"));
            return View((IEnumerable<Albume>)consulta);
        }

        // GET: Albumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = _context.DameTodos().Join(_Grupocontext.DameTodos(), album => album.GruposId,
                    grupo => grupo.Id, (album, grupo) => new { Album = album, Grupo = grupo }).Select(x => x)
                .FirstOrDefault(x => x.Album.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // GET: Albumes/Create
        public IActionResult Create()
        {
            ViewData["GruposId"] = new SelectList(_Grupocontext.DameTodos(), "Id", "Nombre");
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
            ViewData["GruposId"] = new SelectList(_Grupocontext.DameTodos(), "Id", "Nombre", albume.GruposId);
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
            ViewData["GruposId"] = new SelectList(_Grupocontext.DameTodos(), "Id", "Nombre", albume.GruposId);
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
                    _context.Modificar(id,albume);
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
            ViewData["GruposId"] = new SelectList(_Grupocontext.DameTodos(), "Id", "Id", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albume = _context.DameTodos().Join(_Grupocontext.DameTodos(), album => album.GruposId,
                    grupo => grupo.Id, (album, grupo) => new { Album = album, Grupo = grupo }).Select(x => x)
                .FirstOrDefault(x => x.Album.Id == id);
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
