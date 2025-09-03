using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Pedido
    {
        [Key()]
        public int IdPedido { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        //public virtual Cliente? Cliente { get; set; }
    }
}