using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class TipoProduto
    {
        [Key()]
        public int IdTipo { get; set; }
        public string? NomeTipo { get; set; }
    }
}