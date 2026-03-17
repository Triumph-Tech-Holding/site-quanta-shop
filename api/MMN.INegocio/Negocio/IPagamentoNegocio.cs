using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.Util.Model.LionBit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IPagamentoNegocio : IBaseNegocio<PagamentoViewModel, Pagamento>
    {
        public IEnumerable<Pagamento> ObterPagamentos(long IdPedido);
        public IEnumerable<Pagamento> ObterPagamentosUsuario(Guid IdUsuario);
        public Pagamento ObterParcela(int IdPedido, int NumeroParcela);
        public Pagamento PagarParcela(int IdPedido, int NumeroParcela);
        public Pagamento ObterProximaParcela(int IdPedido);
        public void InativarPagamento(Pagamento pagamento);
        public Task ReativarPagamentoAsync(Pagamento pagamento, Pedido pedido);
        public Task<PagamentoViewModel> CriarBoletoAsync(long idPedido, int numeroParcela, Guid IdUsuario, string observacao = "Compra de plano BIGCASH");
        public Task<PagamentoViewModel> CriarBoletoAsync(PagamentoViewModel parcela, Pedido pedido, string observacao = "Compra de plano BIGCASH");
        public Task<IEnumerable<Pagamento>> CriarParcelasAsync(IEnumerable<Pagamento> parcelas, Pedido pedido, bool gerarBoleto = true);

        //public Task<IEnumerable<Pagamento>> CriarParcelaAssinaturaAsync(Pedido pedido, CartaoViewModel card);

        public Task<DadosRetorno<string>> CriarAssinaturaAsync(Pedido pedido, CartaoViewModel card, string observacao = "Assinatura de plano BIGCASH");

        public Task<DadosRetorno<string>> CancelarAssinatura(Pedido pedido);
    }
}
