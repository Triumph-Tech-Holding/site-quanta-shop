using FluentValidation;
using MMN.Dominio.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioViewModel
    {
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioPai { get; set; }
        public int IdGrupo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public byte[] SaltKey { get; set; }
        public string Documento { get; set; }
        public bool Ativo { get; set; }
        public bool TermosDeAceite { get; set; }
        public string Celular { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public short? TentativasIncorretas { get; set; }
        public string AssinaturaEletronica { get; set; }
        public int? IdGraduacao { get; set; }
        public string Cultura { get; set; }
        public short? PosicaoBinario { get; set; }
        public DateTime? DataReferencia { get; set; }
        public DateTime? DataQualificacao { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool Master { get; set; }
        public virtual GraduacaoViewModel Graduacao { get; set; }
        public virtual Grupo Grupo { get; set; }
        public virtual Usuario UsuarioPai { get; set; }
        public string UrlImg { get; set; }
        public string RG { get; set; }
        public string OrgaoEmissorRG { get; set; }
        public string EstadoRG { get; set; }
        public bool Empreendedor { get; set; }
        public char Perfil { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool PreCadastro { get; set; }
        public string NomeSocial { get; set; }
        public string Genero { get; set; }
        public bool LoginAlterado { get; set; }
        public bool IndicadoPeloQS { get; set; }
        public DateTime? DataUltimoAcesso { get; set; }
        public string EnderecoIPUltimoAcesso { get; set; }
        public string AgenteUltimoAcesso { get; set; }
        public string LinkAssistenteVirtual { get; set; }
        public string AsaasCustomerId { get; set; }
        public UsuarioProdutoViewModel ProdutoAtivo { get; set; }
        public ICollection<UsuarioConfiguracao> UsuarioConfiguracao { get; set; }
        public ICollection<CacheResumoBinario> CacheResumoBinario { get; set; }
        public ICollection<HistoricoGraduacao> HistoricoGraduacao { get; set; }
        public ICollection<Usuario> Filhos { get; set; }
        public ICollection<Lancamento> Lancamento { get; set; }
        public ICollection<Transacao> Transacao { get; set; }
        public ICollection<UsuarioBanco> UsuarioBanco { get; set; }
        public ICollection<UsuarioEndereco> UsuarioEndereco { get; set; }
        public ICollection<UsuarioProduto> UsuarioProduto { get; set; }
        public ICollection<UsuarioCarteira> UsuarioCarteira { get; set; }
        public ICollection<PedidoViewModel> Pedido { get; set; }
        public ICollection<PedidoViewModel> Vendas { get; set; }
        public ICollection<FaturaViewModel> Faturas { get; set; }
        public CredenciamentoViewModel Credenciamento { get; set; }
        public ICollection<CredenciamentoViewModel> Credenciamentos { get; set; }
        public string LoginPatrocinador { get; set; }
    }

    public class UsuarioViewModelValidator : AbstractValidator<UsuarioViewModel>
    {
        public UsuarioViewModelValidator()
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("EmailInvalido");
            RuleFor(u => u.Senha).Must(UtilBase.RequisitosSenha).WithMessage("RequisitosSenha");
            RuleFor(u => u.Login).MinimumLength(5).WithMessage("TamanhoMinimoLogin");
        }


    }
}
