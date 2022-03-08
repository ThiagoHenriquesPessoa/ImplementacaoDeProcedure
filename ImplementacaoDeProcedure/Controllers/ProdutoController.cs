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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var param = new SqlParameter("@id", id);

            var produto = _context.Produto.FromSqlRaw("Consultar @id", param).IgnoreQueryFilters().AsNoTracking().AsEnumerable().FirstOrDefault();

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
                var param1 = new SqlParameter("@id", produto.Id);
                var param2 = new SqlParameter("@nome", produto.Nome);

                await _context.Database.ExecuteSqlRawAsync("Cadastro @id, @nome", param1, param2);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var param = new SqlParameter("@id", id);

            var produto = _context.Produto.FromSqlRaw("Consultar @id", param).IgnoreQueryFilters().AsNoTracking().AsEnumerable().FirstOrDefault();

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
                    var param1 = new SqlParameter("@id", produto.Id);
                    var param2 = new SqlParameter("@nome", produto.Nome);

                    await _context.Database.ExecuteSqlRawAsync("EXEC Alterar @id, @nome", param1, param2);

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

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var param = new SqlParameter("@id", id);

            var produto = _context.Produto.FromSqlRaw("Consultar @id", param).IgnoreQueryFilters().AsNoTracking().AsEnumerable().FirstOrDefault();

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

            var produto = _context.Produto.FromSqlRaw("Consultar @id", param).IgnoreQueryFilters().AsNoTracking().AsEnumerable().FirstOrDefault();
            if (produto == null)
            {
                return NotFound();
            }

            await _context.Database.ExecuteSqlRawAsync("Excluir @id", param);

           
           
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
