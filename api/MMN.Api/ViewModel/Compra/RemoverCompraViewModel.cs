namespace MMN.Api.ViewModel.Compra
{
    /// <summary>
    /// Parametros para remover uma compra
    /// </summary>
    public class RemoverCompraViewModel
    {
        /// <summary>
        /// Pedido a ser removido
        /// </summary>
        public long IdPedido { get; set; }
        /// <summary>
        /// Confirmação de que pedido deve ser removido
        /// </summary>
        public bool ConfirmarRemover { get; set; }
    }
}
