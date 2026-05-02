using System;
using MMN.Dominio.Model;

namespace MMN.Negocio.Services
{
    /// <summary>
    /// Logica pura de validacao de cupom — sem dependencia de banco.
    /// Testavel via xUnit sem infraestrutura.
    /// As verificacoes de MaxUsos (necessitam COUNT no banco) continuam no CupomController.
    /// </summary>
    public static class CupomValidacaoHelper
    {
        public record ResultadoValidacao(bool Valido, string Mensagem);

        /// <summary>
        /// Valida um cupom sem consultar o banco.
        /// </summary>
        /// <param name="cupom">Cupom carregado do banco (pode ser null).</param>
        /// <param name="valorPedido">Valor total do pedido (opcional).</param>
        /// <param name="agora">Data/hora de referencia (UTC). Injetavel para facilitar testes.</param>
        public static ResultadoValidacao ValidarRegrasBasicas(Cupom cupom, decimal? valorPedido, DateTime agora)
        {
            if (cupom == null || !cupom.Ativo)
                return new(false, "Cupom invalido ou expirado.");

            if (cupom.ValidoDe.HasValue && cupom.ValidoDe.Value > agora)
                return new(false, "Cupom ainda nao esta ativo.");

            if (cupom.ValidoAte.HasValue && cupom.ValidoAte.Value < agora)
                return new(false, "Cupom expirado.");

            if (valorPedido.HasValue && cupom.MinimoPedido.HasValue && valorPedido.Value < cupom.MinimoPedido.Value)
                return new(false, $"Pedido minimo de R$ {cupom.MinimoPedido.Value:F2} para usar este cupom.");

            return new(true, null);
        }
    }
}
