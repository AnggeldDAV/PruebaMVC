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
    public class ListasCancionesController : Controller
    {
        private readonly IGenericRepositorio<ListasCancione> _context;
        private readonly IGenericRepositorio<Cancione> _contextCanciones;
        private readonly IGenericRepositorio<Lista> _contextListas;
        private readonly IGenericRepositorio<VistaListaCancione> _contextVista;

        public ListasCancionesController(IGenericRepositorio<ListasCancione> context, IGenericRepositorio<Cancione> contextCanciones, IGenericRepositorio<Lista> contextListas, IGenericRepositorio<VistaListaCancione> contextVista)
        {
            _context = context;
            _contextCanciones = contextCanciones;
            _contextListas = contextListas;
            _contextVista = contextVista;
        }

        // GET: ListasCanciones
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await _contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: ListasCanciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var listasCancione = vista.AsParallel()
                .FirstOrDefault(m => m.Id == id);
            if (listasCancione == null)
            {
                return NotFound();
            }

            return View(listasCancione);
        }

        // GET: ListasCanciones/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CancionesId"] = new SelectList(await _contextCanciones.DameTodos(), "Id", "Titulo");
            ViewData["ListasId"] = new SelectList(await _contextListas.DameTodos(), "Id", "Nombre");
            return View();
        }

        // POST: ListasCanciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ListasId,CancionesId")] ListasCancione listasCancione)
        {
            if (ModelState.IsValid)
            {
                await _context.Agregar(listasCancione);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CancionesId"] = new SelectList(await _contextCanciones.DameTodos(), "Id", "Titulo", listasCancione.CancionesId);
            ViewData["ListasId"] = new SelectList(await _contextListas.DameTodos(), "Id", "Nombre", listasCancione.ListasId);
            return View(listasCancione);
        }

        // GET: ListasCanciones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listasCancione = await _context.DameUno(id);
            if (listasCancione == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            ViewData["CancionesId"] = new SelectList(await _contextCanciones.DameTodos(), "Id", "Titulo", listasCancione.CancionesId);
            ViewData["ListasId"] = new SelectList(await _contextListas.DameTodos(), "Id", "Nombre", listasCancione.ListasId);
            return View(conjunto);
        }

        // POST: ListasCanciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ListasId,CancionesId")] ListasCancione listasCancione)
        {
            if (id != listasCancione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Modificar(id,listasCancione);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListasCancioneExists(listasCancione.Id).Result)
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
            ViewData["CancionesId"] = new SelectList(await _contextCanciones.DameTodos(), "Id", "Titulo", listasCancione.CancionesId);
            ViewData["ListasId"] = new SelectList(await _contextListas.DameTodos(), "Id", "Nombre", listasCancione.ListasId);
            return View(conjunto);
        }

        // GET: ListasCanciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await _contextVista.DameTodos();
            var listasCancione = vista.FirstOrDefault(m => m.Id == id);
            if (listasCancione == null)
            {
                return NotFound();
            }

            return View(listasCancione);
        }

        // POST: ListasCanciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listasCancione = await _context.DameUno(id);
            if (listasCancione != null)
            {
                await _context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ListasCancioneExists(int id)
        {
            var vista = await _context.DameTodos();
            return vista.AsParallel().Any(e => e.Id == id);
        }
    }
}
