using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
    public class PedidoProduto
    {
        [Key()]
        public int IdPedidoProduto { get; set; }

        [ForeignKey("Pedido")]
        public int IdPedido { get; set; }

        [ForeignKey("Produto")]
        public int IdProduto { get; set; }
        //public virtual Pedido? Pedido { get; set; }

        public int Quantidade { get; set; }
    }
}