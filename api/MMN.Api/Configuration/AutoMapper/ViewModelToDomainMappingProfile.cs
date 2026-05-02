using AutoMapper;
using MMN.Api.ViewModel.Cupom;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.Dominio.ViewModel.Suporte;

namespace MMN.Dominio.Api.Configuration
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UsuarioViewModel, Usuario>()
                .ForMember(dest => dest.Pedido, opt => opt.MapFrom(src => src.Pedido))
                .ForMember(dest => dest.Graduacao, opt => opt.MapFrom(src => src.Graduacao))
                .ForMember(dest => dest.Vendas, opt => opt.MapFrom(src => src.Vendas))
                .ForMember(dest => dest.Faturas, opt => opt.MapFrom(src => src.Faturas));
            CreateMap<UsuarioAdminViewModel, Usuario>();
            CreateMap<GrupoViewModel, Grupo>();
            CreateMap<GraduacaoViewModel, Graduacao>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.UsuarioViewModel))
                .ForMember(dest => dest.PremiacaoDownline, opt => opt.MapFrom(src => src.PremiacaoDownlineViewModel))
                .ForMember(dest => dest.MensagemGraduacao, opt => opt.MapFrom(src => src.MensagemGraduacaoViewModel))
                .ForMember(dest => dest.GraduacaoRequisitos, opt => opt.MapFrom(src => src.GraduacaoRequisitos));
            CreateMap<MenuViewModel, Menu>();
            CreateMap<LancamentoViewModel, Lancamento>()
                .ForMember(dest => dest.LancamentoRetido, opt => opt.MapFrom(src => src.LancamentoRetido));
            CreateMap<PedidoViewModel, Pedido>()
                .ForMember(dest => dest.PedidoDetalhe, opt => opt.MapFrom(src => src.PedidoDetalhe))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.UsuarioComerciante, opt => opt.MapFrom(opt => opt.UsuarioComerciante));
            CreateMap<PagamentoViewModel, Pagamento>();
            CreateMap<TipoViewModel, Tipo>();
            CreateMap<TransacaoViewModel, Transacao>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusViewModel))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TipoViewModel))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.UsuarioViewModel))
                .ForMember(dest => dest.Saque, opt => opt.MapFrom(src => src.SaqueViewModel))
                .ForMember(dest => dest.Anunciante, opt => opt.MapFrom(src => src.AnuncianteViewModel));
            CreateMap<ConfiguracaoViewModel, Configuracao>();
            CreateMap<EstadoViewModel, Estado>();
            CreateMap<CidadeViewModel, Cidade>();
            CreateMap<UsuarioEnderecoViewModel, UsuarioEndereco>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade));

            CreateMap<BancoViewModel, Banco>();
            CreateMap<ProdutoViewModel, Produto>();
            CreateMap<CategoriaViewModel, Categoria>().ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.Produtos));
            CreateMap<UsuarioBancoViewModel, UsuarioBanco>()
                .ForMember(dest => dest.Banco, opt => opt.MapFrom(src => src.Banco))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo));

            CreateMap<UsuarioProdutoViewModel, UsuarioProduto>()
                .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.Pedido, opt => opt.MapFrom(src => src.Pedido));

            CreateMap<CarrosselViewModel, Carrossel>();

            CreateMap<UsuarioCadastroPWAViewModel, Usuario>();
            CreateMap<UsuarioCadastroViewModel, Usuario>();
            CreateMap<Oauth2CadastroViewModel, Usuario>();
            CreateMap<Oauth2CredentialCadastroViewModel, Usuario>();

            CreateMap<StatusViewModel, Status>();
            CreateMap<UsuarioConfiguracaoViewModel, UsuarioConfiguracao>();
            CreateMap<ProdutoNivelViewModel, ProdutoNivel>();
            CreateMap<SaqueViewModel, Saque>()
                .ForMember(dest => dest.UsuarioBanco, opt => opt.MapFrom(src => src.UsuarioBancoViewModel));
            CreateMap<MensagemGraduacaoViewModel, MensagemGraduacao>()
                .ForMember(dest => dest.Graduacao, opt => opt.MapFrom(src => src.GraduacaoViewModel))
                .ForMember(dest => dest.Mensagem, opt => opt.MapFrom(src => src.MensagemViewModel));

            CreateMap<MensagemViewModel, Mensagem>()
                .ForMember(dest => dest.MensagemGraduacao, opt => opt.MapFrom(src => src.MensagemGraduacao))
                .ForMember(dest => dest.UsuarioDestino, opt => opt.MapFrom(src => src.UsuarioDestino));

            CreateMap<ArquivosViewModel, Arquivos>();
            CreateMap<UsuarioCarteiraViewModel, UsuarioCarteira>();
            CreateMap<FaqViewModel, Faq>();
            CreateMap<GrupoMenuViewModel, GrupoMenu>();
            CreateMap<AnuncianteViewModel, Anunciante>()
                .ForMember(dest => dest.AnuncianteCashBack, opt => opt.MapFrom(src => src.AnuncianteCashBack))
                .ForMember(dest => dest.CategoriaAnunciante, opt => opt.MapFrom(src => src.CategoriaAnunciante));

            CreateMap<PremiacaoDownlineViewModel, PremiacaoDownline>();
            CreateMap<AnuncianteCashBackViewModel, AnuncianteCashBack>();
            CreateMap<CategoriaAnuncianteViewModel, CategoriaAnunciante>();
            CreateMap<CredenciamentoViewModel, Credenciamento>()
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.CidadeViewModel))
                .ForMember(dest => dest.UsuarioPai, opt => opt.MapFrom(src => src.UsuarioPaiViewModel))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.UsuarioViewModel))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.CategoriaViewModel));
            CreateMap<CredenciarViewModel, Credenciamento>();
            CreateMap<GraduacaoRequisitosViewModel, GraduacaoRequisitos>();
            CreateMap<PedidoDetalheViewModel, PedidoDetalhe>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<UsuarioPremiacaoViewModel, UsuarioPremiacao>();
            CreateMap<LancamentoRetidoViewModel, LancamentoRetido>();
            CreateMap<SuporteViewModel, Suporte>()
                .ForMember(dest => dest.SuporteLog, opt => opt.MapFrom(src => src.SuporteLog));
            CreateMap<SuporteCancelarParcelamentoViewModel, Suporte>()
                .ForMember(dest => dest.SuporteLog, opt => opt.MapFrom(src => src.SuporteLog))
                .ForMember(dest => dest.NumeroPedido, opt => opt.MapFrom(src => src.IdPedido))
                .ForMember(dest => dest.SiteCompra, opt => opt.MapFrom(src => src.CodigoPedido));
            CreateMap<SuporteLogViewModel, SuporteLog>();
            CreateMap<AlteracaoPerfilViewModel, AlteracaoPerfil>();
            CreateMap<RefreshTokenViewModel, RefreshToken>();
            CreateMap<FaturaViewModel, Fatura>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.UsuarioViewModel));
            CreateMap<MaterialApoioViewModel, MaterialApoio>();
            CreateMap<EcossistemaViewModel, Ecossistema>();
            CreateMap<QuantaAmizadeViewModel, QuantaAmizade>();
            CreateMap<QuantaAmizadeHistoricoViewModel, QuantaAmizadeHistorico>();
        }
    }
}
