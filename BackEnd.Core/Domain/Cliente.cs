namespace BackEnd.Core.Domain
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string? NomeCliente { get; set; }
        public string? EmailCliente { get; set; }
        public string? NumeroContato { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}