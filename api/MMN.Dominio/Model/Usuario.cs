using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Usuario
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
        public DateTime? DataUltimoAcesso { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool Master { get; set; }
        public string UrlImg { get; set; }
        public string RG { get; set; }
        public bool Empreendedor { get; set; }
        public string OrgaoEmissorRG { get; set; }
        public string EstadoRG { get; set; }
        public char Perfil { get; set; }
        public bool TermosDeAceite { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool PreCadastro { get; set; }
        public string NomeSocial { get; set; }
        public string Genero { get; set; }      
        public bool LoginAlterado { get; set; }
        public bool IndicadoPeloQS { get; set; }
        public string EnderecoIPUltimoAcesso { get; set; }
        public string AgenteUltimoAcesso { get; set; }
        public string LinkAssistenteVirtual { get; set; }
        public string AsaasCustomerId { get; set; }

        public virtual Graduacao Graduacao { get; set; }
        public virtual Grupo Grupo { get; set; }
        public virtual Usuario UsuarioPai { get; set; }
        public virtual ICollection<AutenticacaoExterna> AutenticacaoExterna { get; set; }
        public virtual ICollection<UsuarioConfiguracao> UsuarioConfiguracao { get; set; }
        public virtual ICollection<CacheResumoBinario> CacheResumoBinario { get; set; }
        public virtual ICollection<HistoricoGraduacao> HistoricoGraduacao { get; set; }
        public virtual ICollection<Usuario> Filhos { get; set; }
        public virtual ICollection<Lancamento> Lancamento { get; set; }
        public virtual ICollection<Transacao> Transacao { get; set; }
        public virtual ICollection<UsuarioBanco> UsuarioBanco { get; set; }
        public virtual ICollection<UsuarioEndereco> UsuarioEndereco { get; set; }
        public virtual ICollection<UsuarioProduto> UsuarioProduto { get; set; }
        public virtual ICollection<Saque> Saque { get; set; }
        public virtual ICollection<LogGraduacao> LogGraduacao { get; set; }
        public virtual ICollection<UsuarioCarteira> UsuarioCarteira { get; set; }
        public virtual ICollection<Credenciamento> Credenciamentos { get; set; }
        public virtual Credenciamento Credenciamento { get; set; }
        public virtual ICollection<UsuarioPremiacao> UsuarioPremiacao { get; set; }
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<AuditoriaPremiacao> AuditoriaPremiacao { get; set; }
        public virtual ICollection<Suporte> Suporte { get; set; }
        public virtual ICollection<Suporte> SuporteAdmin { get; set; }
        public virtual ICollection<AlteracaoPerfil> AlteracaoPerfil { get; set; }
        public virtual ICollection<AlteracaoPerfil> AlteracaoPerfilAdmin { get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
        public virtual ICollection<Pedido> Vendas { get; set; }
        public virtual ICollection<Fatura> Faturas { get; set; }
    }
}
