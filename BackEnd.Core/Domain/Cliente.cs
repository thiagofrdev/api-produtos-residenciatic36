using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Context;

namespace BackEnd.Models
{
    public class Cliente
    {
        [Key()]
        public int IdCliente { get; set; }
        public string? NomeCliente { get; set; }
        public string? EmailCliente { get; set; }
        public string? NumeroContato { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}