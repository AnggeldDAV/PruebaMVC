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
    public class UsuariosController : Controller
    {
        private readonly IGenericRepositorio<Usuario> _context;

        public UsuariosController(IGenericRepositorio<Usuario> context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            if (await _context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }
            var vista = await _context.DameTodos();
            var usuarios = vista.Select(x => x);

            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(s => s.Nombre!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "nombre_desc":
                    usuarios = usuarios.OrderByDescending(s => s.Nombre);
                    break;
                case "Email":
                    usuarios = usuarios.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    usuarios = usuarios.OrderByDescending(s => s.Email);
                    break;     
                default:
                    usuarios = usuarios.OrderBy(s => s.Nombre);
                    break;
            }
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.DameUno(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(await usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Contraseña")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await _context.Agregar(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.DameUno(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Contraseña")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   _context.Modificar(id,usuario);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id).Result)
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario =await _context.DameUno(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario =await _context.DameUno(id);
            if (usuario != null)
            {
               await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UsuarioExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
