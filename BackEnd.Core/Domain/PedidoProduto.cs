namespace BackEnd.Core.Domain
{
    public class PedidoProduto
    {
        public int IdPedidoProduto { get; set; }
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
    }
}