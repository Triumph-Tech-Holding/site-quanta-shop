using System;
using MMN.Negocio.Services;
using MMN.Dominio.Model;
using Xunit;

namespace MMN.Tests
{
    /// <summary>
    /// Testes unitarios para CupomValidacaoHelper — logica pura de validacao sem DB.
    /// Protege contra regressao ao refatorar CupomController.
    /// </summary>
    public class CupomValidacaoTests
    {
        private static readonly DateTime Agora = new DateTime(2026, 5, 2, 12, 0, 0, DateTimeKind.Utc);

        private static Cupom CupomValido() => new Cupom
        {
            IdCupom = 1,
            Codigo = "QUANTA10",
            Tipo = "PERCENTUAL",
            Valor = 10m,
            Ativo = true,
            ValidoDe = new DateTime(2026, 1, 1),
            ValidoAte = new DateTime(2026, 12, 31),
            MinimoPedido = 50m,
        };

        // ─── Cupom inativo / nulo ────────────────────────────────────────────────────

        [Fact]
        public void Validar_CupomNulo_RetornaInvalido()
        {
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(null, 100m, Agora);
            Assert.False(r.Valido);
            Assert.Contains("invalido", r.Mensagem, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validar_CupomInativo_RetornaInvalido()
        {
            var cupom = CupomValido();
            cupom.Ativo = false;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 100m, Agora);
            Assert.False(r.Valido);
        }

        // ─── Vigencia ────────────────────────────────────────────────────────────────

        [Fact]
        public void Validar_CupomAindaNaoAtivo_RetornaInvalido()
        {
            var cupom = CupomValido();
            cupom.ValidoDe = Agora.AddDays(1);
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 100m, Agora);
            Assert.False(r.Valido);
            Assert.Contains("ainda nao esta ativo", r.Mensagem, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validar_CupomExpirado_RetornaInvalido()
        {
            var cupom = CupomValido();
            cupom.ValidoAte = Agora.AddDays(-1);
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 100m, Agora);
            Assert.False(r.Valido);
            Assert.Contains("expirado", r.Mensagem, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validar_CupomNaDataLimite_RetornaValido()
        {
            var cupom = CupomValido();
            cupom.ValidoDe = Agora.AddHours(-1);
            cupom.ValidoAte = Agora.AddHours(1);
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 100m, Agora);
            Assert.True(r.Valido);
        }

        [Fact]
        public void Validar_CupomSemVigencia_RetornaValido()
        {
            var cupom = CupomValido();
            cupom.ValidoDe = null;
            cupom.ValidoAte = null;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 100m, Agora);
            Assert.True(r.Valido);
        }

        // ─── Valor minimo ────────────────────────────────────────────────────────────

        [Fact]
        public void Validar_PedidoAbaixoDoMinimo_RetornaInvalido()
        {
            var cupom = CupomValido();
            cupom.MinimoPedido = 100m;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 49.99m, Agora);
            Assert.False(r.Valido);
            Assert.Contains("100,00", r.Mensagem);
        }

        [Fact]
        public void Validar_PedidoExatamenteNoMinimo_RetornaValido()
        {
            var cupom = CupomValido();
            cupom.MinimoPedido = 100m;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 100m, Agora);
            Assert.True(r.Valido);
        }

        [Fact]
        public void Validar_PedidoAcimaDoMinimo_RetornaValido()
        {
            var cupom = CupomValido();
            cupom.MinimoPedido = 50m;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 150m, Agora);
            Assert.True(r.Valido);
        }

        [Fact]
        public void Validar_SemValorPedidoComMinimo_RetornaValido()
        {
            var cupom = CupomValido();
            cupom.MinimoPedido = 100m;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, null, Agora);
            Assert.True(r.Valido);
        }

        [Fact]
        public void Validar_SemMinimoDefinido_RetornaValido()
        {
            var cupom = CupomValido();
            cupom.MinimoPedido = null;
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, 1m, Agora);
            Assert.True(r.Valido);
        }

        // ─── Cupom valido completo ────────────────────────────────────────────────────

        [Fact]
        public void Validar_CupomCompletoValido_RetornaMensagemNula()
        {
            var r = CupomValidacaoHelper.ValidarRegrasBasicas(CupomValido(), 200m, Agora);
            Assert.True(r.Valido);
            Assert.Null(r.Mensagem);
        }
    }
}
