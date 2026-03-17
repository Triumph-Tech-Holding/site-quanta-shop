using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System.Collections.Generic;

namespace MMN.Negocio.Negocio
{
    public class TipoPagamentoNegocio: ITipoPagamentoNegocio
    {
        public IEnumerable<TipoPagamentoViewModel> GetTipoPagamentoVendaOffline()
        {
            return new TipoPagamentoViewModel[]
            {
                new TipoPagamentoViewModel
                {
                    Chave = (int)EnumTipoPagamento.Dinheiro,
                    Valor = EnumTipoPagamento.Dinheiro.GetDescription()
                },
                new TipoPagamentoViewModel
                {
                    Chave = (int)EnumTipoPagamento.Outro,
                    Valor = EnumTipoPagamento.Outro.GetDescription()
                },
                new TipoPagamentoViewModel
                {
                    Chave = (int)EnumTipoPagamento.CartaoDebito,
                    Valor = EnumTipoPagamento.CartaoDebito.GetDescription()
                },
                new TipoPagamentoViewModel
                {
                    Chave = (int)EnumTipoPagamento.CartaoCredito,
                    Valor = EnumTipoPagamento.CartaoCredito.GetDescription()
                },
                new TipoPagamentoViewModel
                {
                    Chave = (int)EnumTipoPagamento.Pix,
                    Valor = EnumTipoPagamento.Pix.GetDescription()
                },
                //new TipoPagamentoViewModel
                //{
                //    Chave = (int)EnumTipoPagamento.Ted,
                //    Valor = EnumTipoPagamento.Ted.GetDescription()
                //},
                //new TipoPagamentoViewModel
                //{
                //    Chave = (int)EnumTipoPagamento.Doc,
                //    Valor = EnumTipoPagamento.Doc.GetDescription()
                //},
                new TipoPagamentoViewModel
                {
                    Chave = (int)EnumTipoPagamento.Saldo,
                    Valor = EnumTipoPagamento.Saldo.GetDescription()
                }
            };
        }
    }
}
