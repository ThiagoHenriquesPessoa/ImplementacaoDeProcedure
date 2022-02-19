using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImplementacaoDeProcedure.Models
{
    public class Produto : Entity
    {        
        [Column("Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }
}
