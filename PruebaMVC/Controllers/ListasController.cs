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
    public class ListasController : Controller
    {
        private readonly IGenericRepositorio<Lista> _context;
        private readonly IGenericRepositorio<Usuario> _contextUsuario;
        private readonly IGenericRepositorio<VistaListum> _contextVista;

        public ListasController(IGenericRepositorio<Lista> context, IGenericRepositorio<Usuario> contextUsuario, IGenericRepositorio<VistaListum> contextVista)
        {
            _context = context;
            _contextUsuario = contextUsuario;
            _contextVista = contextVista;
        }

        // GET: Listas
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await _contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: Listas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var lista = vista.FirstOrDefault(m => m.Id == id);
            if (lista == null)
            {
                return NotFound();
            }

            return View(lista);
        }

        // GET: Listas/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UsuarioId"] = new SelectList(await _contextUsuario.DameTodos(), "Id", "Nombre");
            return View();
        }

        // POST: Listas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,UsuarioId")] Lista lista)
        {
            if (ModelState.IsValid)
            {
                await _context.Agregar(lista);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(await _contextUsuario.DameTodos(), "Id", "Id", lista.UsuarioId);
            return View(lista);
        }

        // GET: Listas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lista = await _context.DameUno(id);
            if (lista == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            ViewData["UsuarioId"] = new SelectList(await _contextUsuario.DameTodos(), "Id", "Nombre", lista.UsuarioId);
            return View(conjunto);
        }

        // POST: Listas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,UsuarioId")] Lista lista)
        {
            if (id != lista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Modificar(id,lista);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListaExists(lista.Id).Result)
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
            ViewData["UsuarioId"] = new SelectList(await _contextUsuario.DameTodos(), "Id", "Id", lista.UsuarioId);
            return View(lista);
        }

        // GET: Listas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var lista = vista
                .FirstOrDefault(m => m.Id == id);
            if (lista == null)
            {
                return NotFound();
            }

            return View(lista);
        }

        // POST: Listas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lista = await _context.DameUno(id);
            if (lista != null)
            {
                await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ListaExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
