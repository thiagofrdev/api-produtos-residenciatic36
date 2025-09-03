using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions options): base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoProduto> TiposProduto { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProduto> PedidosProdutos { get; set; }
    }
}