using MMN.Util.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMN.Dominio.Model
{
    public class Credenciamento
    {
        public long IdCredenciamento { get; set; }
        public string Estabelecimento { get; set; }
        public decimal FaturamentoMensal { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string Telefone { get; set; }
        public string Cnpj { get; set; }
        public StatusCredenciamento Status { get; set; }
        public decimal? PercentualCashback { get; set; }
        public int IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid? IdUsuarioPai { get; set; }
        public virtual Usuario UsuarioPai { get; set; }
        public Guid? IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        public string MotivoRecusa { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataAtivacao { get; set; }
        public string CelularContato { get; set; }

        public string RazaoSocial { get; set; }
        public bool AceitaPgtoComSaldo { get; set; }
        public bool ScanGo { get; set; }
        public string BreveDescricao { get; set; }
        public string DescricaoCompleta { get; set; }
        public int? IdEcossistema { get; set; }

        [ForeignKey("IdEcossistema")]
        public virtual Ecossistema Ecossistema { get; set; }

        public virtual OrdenacaoAnuncio OrdenacaoAnuncio { get; set; }
    }

    public class CredenciamentoCache
    {
        public long IdAnunciante { get; set; }
        public string Nome { get; set; }
        public decimal Cashback { get; set; }
        public string ImagemUrl { get; set; }
        public bool Ativo { get; set; }
        public TipoAnuncianteEnum Tipo { get; set; }
        public int IdCategoria { get; set; }
    }
}
