using BackEnd.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Infrastructure.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoProduto> TiposProduto { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProduto> PedidosProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Define as PKs dos Domains (Antes era feito no próprio Domain)
            modelBuilder.Entity<Cliente>(entity => {entity.HasKey(e => e.IdCliente);});
            modelBuilder.Entity<Pedido>(entity => {entity.HasKey(e => e.IdPedido);});
            modelBuilder.Entity<PedidoProduto>(entity => {entity.HasKey(e => e.IdPedidoProduto);});
            modelBuilder.Entity<Produto>(entity => {entity.HasKey(e => e.IdProduto);});
            modelBuilder.Entity<TipoProduto>(entity => {entity.HasKey(e => e.IdTipo);});
        }
    }
}