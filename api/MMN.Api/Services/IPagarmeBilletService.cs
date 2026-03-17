using System.Threading.Tasks;

namespace MMN.Api.Services
{
    public interface IPagarmeBilletService
    {
        Task<PagarmeBilletResult> CreateBilletAsync(PagarmeBilletRequest request);
    }

    public class PagarmeBilletRequest
    {
        public int PedidoId { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
    }

    public class PagarmeBilletResult
    {
        public bool Success { get; set; }
        public string BoletoUrl { get; set; }
        public string LinhaDigitavel { get; set; }
        public string IdTransacao { get; set; }
        public string MensagemErro { get; set; }
    }
}
