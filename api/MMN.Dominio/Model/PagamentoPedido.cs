namespace MMN.Dominio.Model
{
    public class PagamentoPedido
    {
        public int IdPagamento { get; set; }
        public long IdPedido { get; set; }
        public int? Ordem { get; set; }
        public int Status { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPago { get; set; }

        public virtual Pagamento Pagamento { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}
