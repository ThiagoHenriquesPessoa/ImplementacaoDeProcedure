using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImplementacaoDeProcedure.Data;
using ImplementacaoDeProcedure.Models;
using Microsoft.Data.SqlClient;

namespace ImplementacaoDeProcedure.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProcedureDbContext _context;

        public ProdutoController(ProcedureDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //return View(await _context.Produto.ToListAsync());
            var list = await _context.Produto.FromSqlRaw("ConsultarTodos").ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var param = new SqlParameter("@id", id);

            var produto = await _context.Produto.FromSqlRaw("Consultar @id", param).FirstOrDefaultAsync();

            //var produto = await _context.Produto
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var param = new SqlParameter("@id", id);

            var produto = await _context.Produto.FromSqlRaw("Consultar @id", param).FirstOrDefaultAsync();

            //var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            return View(produto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var param = new SqlParameter("@id", id);

            var produto = await _context.Produto.FromSqlRaw("Consultar @id", param).FirstOrDefaultAsync();

            //var produto = await _context.Produto
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var param = new SqlParameter("@id", id);

            var produto = await _context.Produto.FromSqlRaw("Consultar @id", param).FirstOrDefaultAsync();

            //var produto = await _context.Produto.FindAsync(id);
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            //return _context.Produto.Any(e => e.Id == id);
            var param = new SqlParameter("@id", id);

            var produto =  _context.Produto.FromSqlRaw("Consultar @id", param).Any();

            return produto;
        }
    }
}
