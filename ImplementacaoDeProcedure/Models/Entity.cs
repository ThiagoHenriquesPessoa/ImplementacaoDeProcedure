using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImplementacaoDeProcedure.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public Entity()
        {
            Random num = new Random();            
            Id = num.Next();
        }
    }
}
