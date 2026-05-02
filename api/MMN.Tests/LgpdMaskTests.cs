using MMN.Util.Util;
using Xunit;

namespace MMN.Tests
{
    /// <summary>
    /// Testes unitarios para LgpdMask — helper de mascaramento LGPD (puro, sem DB).
    /// Protege contra regressao nas regras de mascaramento ao refatorar controllers admin.
    /// </summary>
    public class LgpdMaskTests
    {
        // ─── MaskCpfCnpj ────────────────────────────────────────────────────────────

        [Fact]
        public void MaskCpfCnpj_Cpf11Digitos_ExibeApenasBloco3()
        {
            var resultado = LgpdMask.MaskCpfCnpj("123.456.789-09");
            Assert.Equal("***.***.789-**", resultado);
        }

        [Fact]
        public void MaskCpfCnpj_CpfSemFormatacao_MascaraCorretamente()
        {
            var resultado = LgpdMask.MaskCpfCnpj("12345678909");
            Assert.Equal("***.***.789-**", resultado);
        }

        [Fact]
        public void MaskCpfCnpj_Cnpj14Digitos_MascaraCorretamente()
        {
            var resultado = LgpdMask.MaskCpfCnpj("11.222.333/0001-81");
            Assert.Equal("**.***.333/****-**", resultado);
        }

        [Fact]
        public void MaskCpfCnpj_StringVazia_RetornaVazia()
        {
            Assert.Equal("", LgpdMask.MaskCpfCnpj(""));
        }

        [Fact]
        public void MaskCpfCnpj_Null_RetornaNull()
        {
            Assert.Null(LgpdMask.MaskCpfCnpj(null));
        }

        [Fact]
        public void MaskCpfCnpj_DocCurto3Digitos_MascaraTudo()
        {
            var resultado = LgpdMask.MaskCpfCnpj("123");
            Assert.Equal("***", resultado);
        }

        [Fact]
        public void MaskCpfCnpj_DocGenerico8Digitos_ExibeUltimos2()
        {
            var resultado = LgpdMask.MaskCpfCnpj("12345678");
            Assert.Equal("******78", resultado);
        }

        // ─── MaskEmail ───────────────────────────────────────────────────────────────

        [Fact]
        public void MaskEmail_EmailNormal_MascaraUserMantendoDominio()
        {
            var resultado = LgpdMask.MaskEmail("joao.silva@email.com");
            Assert.EndsWith("@email.com", resultado);
            Assert.StartsWith("jo", resultado);
            Assert.Contains("*", resultado);
        }

        [Fact]
        public void MaskEmail_UserDoisCaracteres_MascaraComAsterisco()
        {
            var resultado = LgpdMask.MaskEmail("ab@test.com");
            Assert.Equal("a***@test.com", resultado);
        }

        [Fact]
        public void MaskEmail_UserUmCaracter_MascaraComAsterisco()
        {
            var resultado = LgpdMask.MaskEmail("x@test.com");
            Assert.Equal("x***@test.com", resultado);
        }

        [Fact]
        public void MaskEmail_SemArroba_RetornaOriginal()
        {
            var resultado = LgpdMask.MaskEmail("emailinvalido");
            Assert.Equal("emailinvalido", resultado);
        }

        [Fact]
        public void MaskEmail_Null_RetornaNull()
        {
            Assert.Null(LgpdMask.MaskEmail(null));
        }

        // ─── MaskTelefone ────────────────────────────────────────────────────────────

        [Fact]
        public void MaskTelefone_CelularBrasil11Digitos_ExibeUltimos4()
        {
            var resultado = LgpdMask.MaskTelefone("(11) 99999-1234");
            Assert.Equal("*******1234", resultado);
        }

        [Fact]
        public void MaskTelefone_TelFixo10Digitos_ExibeUltimos4()
        {
            var resultado = LgpdMask.MaskTelefone("1133334444");
            Assert.Equal("******4444", resultado);
        }

        [Fact]
        public void MaskTelefone_NumeroMenorQue4_MascaraTudo()
        {
            var resultado = LgpdMask.MaskTelefone("123");
            Assert.Equal("***", resultado);
        }

        [Fact]
        public void MaskTelefone_Null_RetornaNull()
        {
            Assert.Null(LgpdMask.MaskTelefone(null));
        }

        // ─── MaskConta ───────────────────────────────────────────────────────────────

        [Fact]
        public void MaskConta_ContaComDigito_ExibeUltimos2()
        {
            var resultado = LgpdMask.MaskConta("12345-6");
            Assert.Equal("*****-6", resultado);
        }

        [Fact]
        public void MaskConta_Conta2Chars_MascaraTudo()
        {
            var resultado = LgpdMask.MaskConta("12");
            Assert.Equal("**", resultado);
        }

        [Fact]
        public void MaskConta_ContaLonga_ExibeUltimos2()
        {
            var resultado = LgpdMask.MaskConta("00012345");
            Assert.Equal("******45", resultado);
        }

        [Fact]
        public void MaskConta_Null_RetornaNull()
        {
            Assert.Null(LgpdMask.MaskConta(null));
        }

        // ─── MaskAgencia ─────────────────────────────────────────────────────────────

        [Fact]
        public void MaskAgencia_AgenciaNormal_ExibePrimeiro_MascaraResto()
        {
            var resultado = LgpdMask.MaskAgencia("0001");
            Assert.Equal("0***", resultado);
        }

        [Fact]
        public void MaskAgencia_AgenciaUmChar_RetornaAsterisco()
        {
            var resultado = LgpdMask.MaskAgencia("1");
            Assert.Equal("*", resultado);
        }

        [Fact]
        public void MaskAgencia_Null_RetornaNull()
        {
            Assert.Null(LgpdMask.MaskAgencia(null));
        }
    }
}
