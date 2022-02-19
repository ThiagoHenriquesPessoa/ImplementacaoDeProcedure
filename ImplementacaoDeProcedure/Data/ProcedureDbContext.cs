using ImplementacaoDeProcedure.Models;
using Microsoft.EntityFrameworkCore;

namespace ImplementacaoDeProcedure.Data
{
    public class ProcedureDbContext : DbContext
    {
        public ProcedureDbContext(DbContextOptions<ProcedureDbContext> options) : base(options)
        {
        }
        public DbSet<Produto> Produto { get; set; }
    }
}
