namespace BackEnd.Core.Domain
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public int IdTipoProduto { get; set; }
        public string? NomeProduto { get; set; }
        public decimal PrecoProduto { get; set; }
    }
}