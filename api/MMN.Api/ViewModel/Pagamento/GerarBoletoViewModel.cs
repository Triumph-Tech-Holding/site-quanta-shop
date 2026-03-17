namespace MMN.Api.ViewModel.Pagamento
{
    /// <summary>
    /// Parâmetros para gerar o boleto de um pagamento
    /// </summary>
    public class GerarBoletoViewModel
    {
        /// <summary>
        /// Identificador do pedido a gerar boleto
        /// </summary>
        public int IdPedido { get; set; }
        /// <summary>
        /// Número da parcela a gerar boleto
        /// </summary>
        public int NumeroParcela { get; set; }
    }
}
