using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Produto
    {
        [Key()]
        public int IdProduto { get; set; }

        [ForeignKey("TipoProduto")]
        public int IdTipoProduto { get; set; }
        //public TipoProduto? TipoProduto { get; set; }

        public string? NomeProduto { get; set; }
        public decimal PrecoProduto { get; set; }
    }
}