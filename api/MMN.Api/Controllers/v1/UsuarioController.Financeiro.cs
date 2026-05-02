using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Enum;
using MMN.Dominio.ViewModel;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    public partial class UsuarioController
    {
        [HttpGet]
        [Route("getSaldo")]
        public IActionResult GetSaldo()
        {
            return Ok(new
            {
                saldo = _lancamentoNegocio
                    .Get(l => l.Ativo && l.IdUsuario == IdUsuarioLogado && l.IdStatus != 3 && !l.Bloqueado, "LancamentoRetido")
                    .Sum(l => l.Valor - l.LancamentoRetido.Where(w => w.Ativo).Sum(lt => lt.Valor))
            });
        }

        [HttpPost]
        [Route("verificarSaldo")]
        public IActionResult VerificarSaldo(VerificarSaldoViewModel venda)
        {
            var mensagens = new List<string>();
            var saldoSuficiente = false;

            var saldo = _lancamentoNegocio
                .Get(l => l.Ativo && l.IdUsuario == IdUsuarioLogado && l.IdStatus != 3 && !l.Bloqueado, "LancamentoRetido")
                .Sum(l => l.Valor - l.LancamentoRetido.Where(w => w.Ativo).Sum(lt => lt.Valor));

            var cashbackAcumulado = _lancamentoNegocio
                .Get(cm => cm.IdUsuario == IdUsuarioLogado && (cm.IdTipo == 33 || cm.IdTipo == 52))
                .Sum(v => v.Valor);

            var valorMinimo = Convert.ToDecimal(_configNegocio.FirstNoTracking(c => c.Chave == "VALOR_MINIMO_CONSUMO").Valor);
            var valorTarifa = Convert.ToDecimal(_configNegocio.FirstNoTracking(c => c.Chave == "TARIFA_PAGAMENTO_COM_SALDO_COMPRADOR").Valor);

            if (saldo >= venda.ValorCompra + valorTarifa)
            {
                if (cashbackAcumulado >= valorMinimo)
                {
                    saldoSuficiente = true;
                }
                else
                {
                    saldoSuficiente = false;
                    mensagens.Add($"Seu cashback acumulado ainda não atingiu o valor mínimo de {valorMinimo:C2}. Faltam {valorMinimo - cashbackAcumulado:C2}.");
                }
            }
            else
            {
                if (valorTarifa > 0)
                    mensagens.Add($"Seu saldo é insuficiente para esta compra, por favor selecione outro método de pagamento. Considere também o valor da tarifa ({valorTarifa:C2}) ao calcular o total necessário.");
                else
                    mensagens.Add("Seu saldo é insuficiente para esta compra, por favor selecione outro método de pagamento");
            }

            return Ok(new
            {
                saldo,
                mensagens,
                saldoSuficiente,
                valorMinimo = valorMinimo.ToString("C2"),
                valorTarifa = valorTarifa.ToString("C2")
            });
        }

        [HttpGet]
        [Route("getGanhos")]
        public async Task<IActionResult> GetGanhos()
        {
            return Ok(new
            {
                ganhos = _lancamentoNegocio
                    .Get(l => l.Ativo && l.Valor > 0 && l.IdUsuario == IdUsuarioLogado && l.IdStatus != (int)StatusTransacaoEnum.Cancelada)
                    .Sum(l => l.Valor)
            });
        }
    }
}
