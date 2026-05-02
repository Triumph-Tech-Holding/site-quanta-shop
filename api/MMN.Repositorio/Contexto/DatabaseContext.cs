using System;
using System.Linq;
using MMN.Dominio.Model;
using Microsoft.EntityFrameworkCore;
using MMN.Repositorio.Mapping;
using MMN.Util.Extensions;
using MMN.Repositorio.Seed;

namespace MMN.Repositorio.Contexto
{
    public class DatabaseContext : DbContext
    {


        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Carrossel> Carrossel { get; set; }
        public virtual DbSet<Cidade> Cidade { get; set; }
        public virtual DbSet<Configuracao> Configuracao { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Faq> Faq { get; set; }
        public virtual DbSet<Graduacao> Graduacao { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<GrupoMenu> GrupoMenu { get; set; }
        public virtual DbSet<HistoricoGraduacao> HistoricoGraduacao { get; set; }
        public virtual DbSet<Lancamento> Lancamento { get; set; }
        public virtual DbSet<Mensagem> Mensagem { get; set; }
        public virtual DbSet<MensagemGraduacao> MensagemGraduacao { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Pagamento> Pagamento { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<CupomCashback> CuponCashback { get; internal set; }
        public virtual DbSet<CuponCashbackPedido> CuponCashbackPedido { get; internal set; }
        public virtual DbSet<Transacao> Transacao { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioEndereco> UsuarioEndereco { get; set; }
        public virtual DbSet<UsuarioProduto> UsuarioProduto { get; set; }
        public virtual DbSet<Banco> Banco { get; set; }
        public virtual DbSet<UsuarioBanco> UsuarioBanco { get; set; }
        public virtual DbSet<UsuarioConfiguracao> UsuarioConfiguracao { get; set; }
        public virtual DbSet<ProdutoNivel> ProdutoNivel { get; set; }
        public virtual DbSet<Anunciante> Anunciante { get; set; }
        public virtual DbSet<OrdenacaoAnuncio> OrdenacaoAnuncio { get; set; }
        public virtual DbSet<PremiacaoDownline> PremiacaoDownline { get; set; }
        public virtual DbSet<AnuncianteCashBack> AnuncianteCashBack { get; set; }
        public virtual DbSet<AnuncianteCashBackLog> AnuncianteCashBackLog { get; set; }
        public virtual DbSet<CategoriaAnunciante> CategoriaAnunciante { get; set; }
        public virtual DbSet<Credenciamento> Credenciamento { get; set; }
        public virtual DbSet<PedidoDetalhe> PedidoDetalhe { get; set; }
        public virtual DbSet<GraduacaoRequisitos> GraduacaoRequisitos { get; set; }
        public virtual DbSet<UsuarioPremiacao> UsuarioPremiacao { get; set; }
        public virtual DbSet<AuditoriaPremiacao> AuditoriaPremiacao { get; set; }
        public virtual DbSet<LancamentoRetido> LancamentoRetido { get; set; }
        public virtual DbSet<Suporte> Suporte { get; set; }
        public virtual DbSet<SuporteLog> SuporteLog { get; set; }
        public virtual DbSet<AlteracaoPerfil> AlteracaoPerfil { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Fatura> Fatura { get; set; }
        public virtual DbSet<Parceiro> Parceiro { get; set; }
        public virtual DbSet<LogProduto> LogProduto { get; set; }
        public virtual DbSet<Tutorial> Tutorial { get; set; }
        public virtual DbSet<MaterialApoio> MaterialApoio { get; set; }
        public virtual DbSet<ProvedorAutenticacao> ProvedorAutenticacao { get; set; }
        public virtual DbSet<AutenticacaoExterna> AutenticacaoExterna { get; set; }
        public virtual DbSet<PagamentoPedido> PagamentoPedido { get; set; }
        public virtual DbSet<PercentualBonusCredenciamento> PercentualBonusCredenciamento { get; set; }
        public virtual DbSet<PercentualBonusResidualCredenciamento> PercentualBonusResidualCredenciamento { get; set; }
        public virtual DbSet<PercentualResidualCashback> PercentualResidualCashback { get; set; }
        public virtual DbSet<CupomCashbackDadosNF> CupomCashbackDadosNF { get; set; }
        public virtual DbSet<CupomCashbackItemNF> CupomCashbackItemNF { get; set; }
        public virtual DbSet<QuantaAmizade> QuantaAmizade { get; set; }
        public virtual DbSet<QuantaAmizadeHistorico> QuantaAmizadeHistorico { get; set; }
        public virtual DbSet<Objetivo> Objetivo { get; set; }
        public virtual DbSet<ObjetivoUsuario> ObjetivoUsuario { get; set; }

        public virtual DbSet<Promocao> Promocao { get; set; }

        // Wave 2 — Motor Financeiro
        public virtual DbSet<Cupom> Cupom { get; set; }
        public virtual DbSet<CupomUso> CupomUso { get; set; }
        public virtual DbSet<QuantaPontoLancamento> QuantaPontoLancamento { get; set; }
        public virtual DbSet<AuditoriaLgpd> AuditoriaLgpd { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CacheResumoBinarioMapping());
            modelBuilder.ApplyConfiguration(new PagamentoMapping());
            modelBuilder.ApplyConfiguration(new CategoriaMapping());
            modelBuilder.ApplyConfiguration(new CidadeMapping());
            modelBuilder.ApplyConfiguration(new ConfiguracaoMapping());
            modelBuilder.ApplyConfiguration(new EstadoMapping());
            modelBuilder.ApplyConfiguration(new FaqMapping());
            modelBuilder.ApplyConfiguration(new GraduacaoMapping());
            modelBuilder.ApplyConfiguration(new GrupoMapping());
            modelBuilder.ApplyConfiguration(new GrupoMenuMapping());
            modelBuilder.ApplyConfiguration(new HistoricoGraduacaoMapping());
            modelBuilder.ApplyConfiguration(new LancamentoMapping());
            modelBuilder.ApplyConfiguration(new MensagemMapping());
            modelBuilder.ApplyConfiguration(new MenuMapping());
            modelBuilder.ApplyConfiguration(new PedidoMapping());
            modelBuilder.ApplyConfiguration(new ProdutoMapping());
            modelBuilder.ApplyConfiguration(new TipoMapping());
            modelBuilder.ApplyConfiguration(new CuponCashbackMapping());
            modelBuilder.ApplyConfiguration(new CuponCashbackPedidoMapping());
            modelBuilder.ApplyConfiguration(new TransacaoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioEnderecoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new UsuarioProdutoMapping());
            modelBuilder.ApplyConfiguration(new BancoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioBancoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracaoMapping());
            modelBuilder.ApplyConfiguration(new SaqueMapping());
            modelBuilder.ApplyConfiguration(new MensagemGraduacaoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioCarteiraMapping());
            modelBuilder.ApplyConfiguration(new ProdutoNivelMapping());
            modelBuilder.ApplyConfiguration(new AnuncianteMapping());
            modelBuilder.ApplyConfiguration(new OrdenacaoAnuncioMapping());
            modelBuilder.ApplyConfiguration(new PremiacaoDownlineMapping());
            modelBuilder.ApplyConfiguration(new AnuncianteCashbackMapping());
            modelBuilder.ApplyConfiguration(new CategoriaAnuncianteMapping());
            modelBuilder.ApplyConfiguration(new CredenciamentoMapping());
            modelBuilder.ApplyConfiguration(new PedidoDetalheMapping());
            modelBuilder.ApplyConfiguration(new GraduacaoRequisitosMapping());
            modelBuilder.ApplyConfiguration(new UsuarioPremiacaoMapping());
            modelBuilder.ApplyConfiguration(new AuditoriaPremiacaoMapping());
            modelBuilder.ApplyConfiguration(new LancamentoRetidoMapping());
            modelBuilder.ApplyConfiguration(new SuporteMapping());
            modelBuilder.ApplyConfiguration(new SuporteLogMapping());
            modelBuilder.ApplyConfiguration(new AlteracaoPerfilMapping());
            modelBuilder.ApplyConfiguration(new RefreshTokenMapping());
            modelBuilder.ApplyConfiguration(new FaturaMapping());
            modelBuilder.ApplyConfiguration(new MaterialApoioMapping());
            modelBuilder.ApplyConfiguration(new ProvedorAutenticacaoMapping());
            modelBuilder.ApplyConfiguration(new AutenticacaoExternaMapping());
            modelBuilder.ApplyConfiguration(new CarrosselMapping());
            modelBuilder.ApplyConfiguration(new LogProdutoMapping());
            modelBuilder.ApplyConfiguration(new TutorialMapping());
            modelBuilder.ApplyConfiguration(new AnuncianteCashbackLogMapping());
            modelBuilder.ApplyConfiguration(new LogGraduacaoMapping());
            modelBuilder.ApplyConfiguration(new ParceiroMapping());
            modelBuilder.ApplyConfiguration(new StatusMapping());
            modelBuilder.ApplyConfiguration(new PagamentoPedidoMapping());
            modelBuilder.ApplyConfiguration(new PercentualBonusCredenciamentoMapping());
            modelBuilder.ApplyConfiguration(new PercentualBonusResidualCredenciamentoMapping());
            modelBuilder.ApplyConfiguration(new PercentualResidualCashbackMapping());
            modelBuilder.ApplyConfiguration(new QuantaAmizadeMapping());

            // Wave 2 — Motor Financeiro
            modelBuilder.ApplyConfiguration(new CupomMapping());
            modelBuilder.ApplyConfiguration(new CupomUsoMapping());
            modelBuilder.ApplyConfiguration(new QuantaPontoLancamentoMapping());
            modelBuilder.ApplyConfiguration(new AuditoriaLgpdMapping());

            modelBuilder.SeedProvedorAutenticacao();
            modelBuilder.SeedCupons();

            modelBuilder.Entity<Usuario>(entry =>
            {
                entry.ToTable("Usuario", tb => tb.HasTrigger("QuantaAmizadeTrigger"));
                entry.ToTable("Usuario", tb => tb.HasTrigger("ObjetivoIndicacaoTrigger"));
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies(false);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DataCadastro").CurrentValue = DateTime.UtcNow.HorarioBrasilia();

                if (entry.State == EntityState.Modified)
                    entry.Property("DataCadastro").IsModified = false;
            }

            return base.SaveChanges();
        }
    }
}
