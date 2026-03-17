using AutoMapper;
using MMN.Api.ViewModel.Cupom;
using MMN.Api.ViewModel.Fatura;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.Dominio.ViewModel.Suporte;
using System.Linq;

namespace MMN.Dominio.Api.Configuration
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(dest => dest.Pedido, opt => opt.MapFrom(src => src.Pedido))
                .ForMember(dest => dest.Graduacao, opt => opt.MapFrom(src => src.Graduacao))
                .ForMember(dest => dest.Vendas, opt => opt.MapFrom(src => src.Vendas))
                .ForMember(dest => dest.Faturas, opt => opt.MapFrom(src => src.Faturas));
            CreateMap<Usuario, UsuarioAdminViewModel>();
            CreateMap<Grupo, GrupoViewModel>();
            CreateMap<Graduacao, GraduacaoViewModel>()
                .ForMember(dest => dest.UsuarioViewModel, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.PremiacaoDownlineViewModel, opt => opt.MapFrom(src => src.PremiacaoDownline))
                .ForMember(dest => dest.MensagemGraduacaoViewModel, opt => opt.MapFrom(src => src.MensagemGraduacao))
                .ForMember(dest => dest.GraduacaoRequisitos, opt => opt.MapFrom(src => src.GraduacaoRequisitos));
            CreateMap<Menu, MenuViewModel>();
            CreateMap<Lancamento, LancamentoViewModel>();
            CreateMap<Pedido, PedidoViewModel>()
                .ForMember(dest => dest.PedidoDetalhe, opt => opt.MapFrom(src => src.PedidoDetalhe))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.UsuarioComerciante, opt => opt.MapFrom(src => src.UsuarioComerciante));
            CreateMap<Pagamento, PagamentoViewModel>();
            CreateMap<Tipo, TipoViewModel>();
            CreateMap<Transacao, TransacaoViewModel>()
                .ForMember(dest => dest.StatusViewModel, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TipoViewModel, opt => opt.MapFrom(src => src.Tipo))
                .ForMember(dest => dest.UsuarioViewModel, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.SaqueViewModel, opt => opt.MapFrom(src => src.Saque))
                .ForMember(dest => dest.AnuncianteViewModel, opt => opt.MapFrom(src => src.Anunciante));
            CreateMap<Configuracao, ConfiguracaoViewModel>();
            CreateMap<Estado, EstadoViewModel>();
            CreateMap<Cidade, CidadeViewModel>();
            CreateMap<UsuarioEndereco, UsuarioEnderecoViewModel>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade));

            CreateMap<Produto, ProdutoViewModel>();
            CreateMap<UsuarioProduto, UsuarioProdutoViewModel>()
                .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.Pedido, opt => opt.MapFrom(src => src.Pedido));

            CreateMap<Categoria, CategoriaViewModel>().ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.Produtos));

            CreateMap<Banco, BancoViewModel>();
            CreateMap<UsuarioBanco, UsuarioBancoViewModel>()
                .ForMember(dest => dest.Banco, opt => opt.MapFrom(src => src.Banco))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo));

            CreateMap<Carrossel, CarrosselViewModel>();

            CreateMap<Status, StatusViewModel>();
            CreateMap<UsuarioConfiguracao, UsuarioConfiguracaoViewModel>();
            CreateMap<ProdutoNivel, ProdutoNivelViewModel>();
            CreateMap<Saque, SaqueViewModel>()
                .ForMember(dest => dest.UsuarioBancoViewModel, opt => opt.MapFrom(src => src.UsuarioBanco));
            CreateMap<MensagemGraduacao, MensagemGraduacaoViewModel>()
                .ForMember(dest => dest.GraduacaoViewModel, opt => opt.MapFrom(src => src.Graduacao))
                .ForMember(dest => dest.MensagemViewModel, opt => opt.MapFrom(src => src.Mensagem));

            CreateMap<Mensagem, MensagemViewModel>()
                .ForMember(dest => dest.MensagemGraduacao, opt => opt.MapFrom(src => src.MensagemGraduacao))
                .ForMember(dest => dest.UsuarioDestino, opt => opt.MapFrom(src => src.UsuarioDestino));

            CreateMap<Arquivos, ArquivosViewModel>();
            CreateMap<UsuarioCarteira, UsuarioCarteiraViewModel>();
            CreateMap<Faq, FaqViewModel>();
            CreateMap<GrupoMenu, GrupoMenuViewModel>();
            CreateMap<Anunciante, AnuncianteViewModel>()
                .ForMember(dest => dest.AnuncianteCashBack, opt => opt.MapFrom(src => src.AnuncianteCashBack))
                .ForMember(dest => dest.CategoriaAnunciante, opt => opt.MapFrom(src => src.CategoriaAnunciante));

            CreateMap<PremiacaoDownline, PremiacaoDownlineViewModel>();
            CreateMap<AnuncianteCashBack, AnuncianteCashBackViewModel>();
            CreateMap<CategoriaAnunciante, CategoriaAnuncianteViewModel>();
            CreateMap<Credenciamento, CredenciamentoViewModel>()
                .ForMember(dest => dest.CidadeViewModel, opt => opt.MapFrom(src => src.Cidade))
                .ForMember(dest => dest.UsuarioPaiViewModel, opt => opt.MapFrom(src => src.UsuarioPai))
                .ForMember(dest => dest.UsuarioViewModel, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.CategoriaViewModel, opt => opt.MapFrom(src => src.Categoria));
            CreateMap<GraduacaoRequisitos, GraduacaoRequisitosViewModel>();
            CreateMap<PedidoDetalhe, PedidoDetalheViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<UsuarioPremiacao, UsuarioPremiacaoViewModel>();
            CreateMap<LancamentoRetido, LancamentoRetidoViewModel>();
            CreateMap<Suporte, SuporteViewModel>()
                .ForMember(dest => dest.SuporteLog, opt => opt.MapFrom(src => src.SuporteLog));
            CreateMap<Suporte, SuporteCancelarParcelamentoViewModel>()
                .ForMember(dest => dest.SuporteLog, opt => opt.MapFrom(src => src.SuporteLog))
                .ForMember(dest => dest.IdPedido, opt => opt.MapFrom(src => src.NumeroPedido))
                .ForMember(dest => dest.CodigoPedido, opt => opt.MapFrom(src => src.SiteCompra));
            CreateMap<SuporteLog, SuporteLogViewModel>();
            CreateMap<AlteracaoPerfil, AlteracaoPerfilViewModel>();
            CreateMap<RefreshToken, RefreshTokenViewModel>();
            CreateMap<Fatura, FaturaViewModel>()
                .ForMember(dest => dest.UsuarioViewModel, opt => opt.MapFrom(src => src.Usuario));
            CreateMap<Tutorial, TutorialViewModel>();
            CreateMap<MaterialApoio, MaterialApoioViewModel>();
            CreateMap<CupomCashback, CupomCashbackViewModel>()
                .ForMember(f => f.IdPedido, opt => opt.MapFrom(m => m.CuponCashbackPedido.IdPedido))
                .ForMember(f => f.Valor, opt => opt.MapFrom(m => (int)(m.Valor * 100)))
                .ForMember(f => f.ValorCashback, opt => opt.MapFrom(m => (int)(m.Valor * m.PercentualCashback * decimal.Parse("0,25") * 100)))
                .ForMember(f => f.PercentualCashback, opt => opt.MapFrom(m => m.PercentualCashback * 100))
                .ForMember(f => f.LoginComerciante, opt => opt.MapFrom(m => m.Comerciante.Login))
                .ForMember(f => f.LogoComerciante, opt => opt.MapFrom(m => m.Comerciante.Credenciamento != null ? m.Comerciante.Credenciamento.LogoUrl : null))
                .ForMember(f => f.NomeComerciante, opt => opt.MapFrom(m => m.Comerciante.Credenciamento != null ? m.Comerciante.Credenciamento.Estabelecimento : m.Comerciante.Nome));

            CreateMap<Pedido, PedidoFaturaViewModel>();
            CreateMap<Pedido, ListaFaturaViewModel>()
                .ForMember(f=>f.DataVencimento, opt=>opt.MapFrom(m=>m.Pagamentos.FirstOrDefault().DataValidade))
                .ForMember(f=>f.UrlBoleto, opt=>opt.MapFrom(m=>m.Pagamentos.FirstOrDefault().UrlBoleto));

            CreateMap<Ecossistema, EcossistemaViewModel>();
            CreateMap<QuantaAmizade, QuantaAmizadeViewModel>();
            CreateMap<QuantaAmizadeHistorico, QuantaAmizadeHistoricoViewModel>();
        }
    }
}
