using AutoMapper;
using Microsoft.Extensions.Options;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class CredenciamentoNegocio : BaseNegocio<CredenciamentoViewModel, Credenciamento>, ICredenciamentoNegocio
    {
        private readonly ICredenciamentoRepositorio _repositorio;
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IUsuarioProdutoRepositorio _usuarioProdutoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUsuarioEnderecoRepositorio _usuarioEnderecoRepositorio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly ICidadeNegocio _cidadeNegocio;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly IConfiguracaoNegocio _configNegocio;

        public CredenciamentoNegocio(
            ICredenciamentoRepositorio repositorio,
            IGrupoRepositorio grupoRepositorio,
            IUsuarioProdutoRepositorio usuarioProdutoRepositorio,
            IPedidoRepositorio pedidoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IUsuarioEnderecoRepositorio usuarioEnderecoRepositorio,
            IMapper mapper,
            IUsuarioNegocio usuarioNegocio,
            ICidadeNegocio cidadeNegocio,
            ICategoriaNegocio categoriaNegocio,
            IOptions<AppSettings> appSettings,
            ICache cache,
            IConfiguracaoNegocio configNegocio) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _grupoRepositorio = grupoRepositorio;
            _usuarioProdutoRepositorio = usuarioProdutoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _usuarioNegocio = usuarioNegocio;
            _usuarioEnderecoRepositorio = usuarioEnderecoRepositorio;
            _cidadeNegocio = cidadeNegocio;
            _categoriaNegocio = categoriaNegocio;
            _appSettings = appSettings.Value;
            _cache = cache;
            _configNegocio = configNegocio;
        }

        public object FiltrarCredenciamento(FiltroViewModel.FiltroCredenciamento filtroCredenciamento)
        {
            return _repositorio.BuscarPorEstabelecimento(filtroCredenciamento);
        }

        public UsuarioViewModel CredenciarNovoUsuario(CredenciarNovoUsuarioViewModel viewModel)
        {
            Usuario usuario = null;
            UsuarioEndereco endereco = null;
            Credenciamento credenciamento = null;

            viewModel.CelularContato = UtilBase.FiltrarDigitos(viewModel.CelularContato);
            viewModel.Cep = UtilBase.FiltrarDigitos(viewModel.Cep);
            viewModel.Cnpj = UtilBase.FiltrarDigitos(viewModel.Cnpj);

            var usuarioCadastro = new UsuarioCadastroViewModel
            {
                Celular = viewModel.Telefone,
                Email = viewModel.Email,
                Login = viewModel.LoginResponsavel,
                LoginPatrocinador = viewModel.LoginPatrocinador,
                Nome = viewModel.NomeResponsavel,
                Senha = viewModel.SenhaResponsavel,
                Documento = viewModel.Cnpj

            };

            var credenciar = new CredenciarViewModel
            {
                Bairro = viewModel.Bairro,
                CelularContato = viewModel.CelularContato,
                Cep = UtilBase.FiltrarDigitos(viewModel.Cep),
                Complemento = viewModel.Complemento,
                ConfirmEmail = viewModel.Email,
                Documento = UtilBase.FiltrarDigitos(viewModel.Cnpj),
                Email = viewModel.Email,
                Estabelecimento = viewModel.Estabelecimento,
                IdCategoria = viewModel.IdCategoria,
                IdCidade = viewModel.IdCidade,
                ImageBase64 = viewModel.ImageBase64,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                NomeResponsavel = viewModel.NomeResponsavel,
                Numero = viewModel.Numero,
                PercentualCashback = viewModel.PercentualCashback,
                Rua = viewModel.Rua
            };

            ValidarNovoCredenciamento(credenciar, null);

            try
            {
                usuario = _usuarioNegocio.Registrar(usuarioCadastro);
                usuario.Documento = credenciar.Documento;

                endereco = new UsuarioEndereco
                {
                    Bairro = credenciar.Bairro,
                    Cep = credenciar.Cep,
                    Complemento = credenciar.Complemento,
                    IdCidade = credenciar.IdCidade,
                    Numero = credenciar.Numero,
                    Rua = credenciar.Rua,
                    Usuario = usuario
                };
                _usuarioEnderecoRepositorio.Insert(endereco);

                credenciamento = Credenciar(credenciar, usuario);

                usuario.UrlImg = credenciamento.LogoUrl;
                _usuarioRepositorio.Update(usuario);
                _usuarioRepositorio.SaveChanges();
            }
            catch
            {
                if (credenciamento?.IdCredenciamento > 0)
                {
                    _repositorio.DeleteRange(new List<Credenciamento> { credenciamento });

                }
                if (usuario != null && usuario.IdUsuario != default(Guid))
                {
                    _usuarioProdutoRepositorio.DeleteRange(usuario.Pedido.SelectMany(s => s.UsuarioProduto).ToList());
                    _pedidoRepositorio.DeleteRange(usuario.Pedido.ToList());
                    _usuarioEnderecoRepositorio.DeleteRange(new List<UsuarioEndereco> { endereco });
                    _usuarioRepositorio.DeleteRange(new List<Usuario> { usuario });
                    _usuarioRepositorio.SaveChanges();
                }

                throw;
            }

            return _mapper.Map<UsuarioViewModel>(usuario);
        }

        public Credenciamento Credenciar(CredenciarViewModel viewModel, Usuario usuario)
        {
            viewModel.CelularContato = UtilBase.FiltrarDigitos(viewModel.CelularContato);
            viewModel.Cep = UtilBase.FiltrarDigitos(viewModel.Cep);
            viewModel.Documento = UtilBase.FiltrarDigitos(viewModel.Documento);

            ValidarNovoCredenciamento(viewModel, usuario.IdUsuario);

            var credenciamento = _mapper.Map<Credenciamento>(viewModel);

            credenciamento.IdUsuario = usuario.IdUsuario;
            credenciamento.IdUsuarioPai = usuario.IdUsuarioPai;
            credenciamento.DataCadastro = DateTime.Now.HorarioBrasilia();
            credenciamento.Status = StatusCredenciamento.Pendente;
            credenciamento.Cep = UtilBase.FiltrarDigitos(viewModel.Cep);
            credenciamento.Cnpj = UtilBase.FiltrarDigitos(viewModel.Documento);
            credenciamento.Telefone = UtilBase.FiltrarDigitos(viewModel.CelularContato);
            credenciamento.CelularContato = UtilBase.FiltrarDigitos(viewModel.CelularContato);
            credenciamento.Latitude = viewModel.Latitude;
            credenciamento.Longitude = viewModel.Longitude;
            credenciamento.LogoUrl = SalvaLogo(viewModel.ImageBase64, usuario.IdUsuario.ToString());
            credenciamento.LogoUrl = credenciamento.LogoUrl ?? "https://bigcash.blob.core.windows.net/imagens-credenciamento/6886c534-1f67-4e5f-8273-c37544ed36622024-05-07-15-19-13.jpeg";
            credenciamento.OrdenacaoAnuncio = new OrdenacaoAnuncio { IdCredenciamento = credenciamento.IdCredenciamento, Ordenacao = 0 };

            var grupoComerciante = _grupoRepositorio.FirstNoTracking(g => g.Descricao == "Comerciante");

            usuario.IdGrupo = grupoComerciante.IdGrupo;
            usuario.Perfil = 'C';

            _usuarioRepositorio.Update(usuario);
            _repositorio.Insert(credenciamento);
            _repositorio.SaveChanges();

            return credenciamento;
        }

        public Credenciamento Credenciar(CredenciarViewModel viewModel, Guid IdUsuarioLogado)
        {
            var usuario = _usuarioRepositorio.GetById(IdUsuarioLogado);

            return Credenciar(viewModel, usuario);
        }

        private void ValidarNovoCredenciamento(CredenciarViewModel viewModel, Guid? IdUsuarioLogado)
        {
            var exceptions = new List<PadraoException>();

            if (viewModel.PercentualCashback <= 0)
            {
                exceptions.Add(new PadraoException("cashback_percentual_minimo"));
            }

            var credenciamentoJaRecebido = Get(c =>
                c.IdUsuario == IdUsuarioLogado ||
                c.Telefone == viewModel.CelularContato ||
                c.Email == viewModel.Email ||
                c.Cnpj == viewModel.Documento ||
                (c.Estabelecimento.ToLower() == viewModel.Estabelecimento.ToLower() &&
                    c.IdCategoria == viewModel.IdCategoria));

            if (credenciamentoJaRecebido.Any(a => a.IdUsuario == IdUsuarioLogado))
            {
                throw new PadraoException("credenciamento_recebido");
            }
            if (credenciamentoJaRecebido.Any(c => UtilBase.FiltrarDigitos(c.Telefone) == UtilBase.FiltrarDigitos(viewModel.CelularContato)))
            {
                exceptions.Add(new PadraoException("telefone_em_uso"));
            }
            else if (credenciamentoJaRecebido.Any(c => c.Email == viewModel.Email))
            {
                exceptions.Add(new PadraoException("email_em_uso"));
            }
            else if (credenciamentoJaRecebido.Any(c => UtilBase.FiltrarDigitos(c.Cnpj) == UtilBase.FiltrarDigitos(viewModel.Documento)))
            {
                exceptions.Add(new PadraoException("cpf_cnpj_em_uso"));
            }
            else if (credenciamentoJaRecebido.Any(c => c.Estabelecimento == viewModel.Estabelecimento &&
                c.IdCategoria == viewModel.IdCategoria))
            {
                exceptions.Add(new PadraoException("estabelecimento_em_uso"));
            }

            var cidade = _cidadeNegocio.First(c => c.IdCidade == viewModel.IdCidade);
            if (cidade == null)
                exceptions.Add(new NotFoundException("cidade_nao_encontrada"));

            var categoria = _categoriaNegocio.First(c => c.IdCategoria == viewModel.IdCategoria);
            if (categoria == null)
                exceptions.Add(new NotFoundException("categoria_nao_encontrada"));

            var usuarioLogin = _usuarioNegocio
                .GetNoTracking(u => u.IdUsuario != IdUsuarioLogado && u.Ativo && (
                    u.Email == viewModel.Email ||
                    u.Documento == viewModel.Documento ||
                    u.Celular == viewModel.CelularContato));

            if (usuarioLogin.Any(a => a.Email == viewModel.Email))
            {
                exceptions.Add(new PadraoException("email_em_uso"));
            }

            if (usuarioLogin.Any(a => a.Documento == viewModel.Documento))
            {
                exceptions.Add(new PadraoException("cpf_cnpj_em_uso"));
            }

            if (usuarioLogin.Any(a => a.Celular == viewModel.CelularContato))
            {
                exceptions.Add(new PadraoException("telefone_em_uso"));
            }

            if (exceptions.Count() == 1)
            {
                throw exceptions.First();
            }
            else if (exceptions.Count() > 1)
            {
                throw new AggregateException(exceptions);
            }
        }

        public void Atualizar(CredenciamentoViewModel viewModel)
        {
            var credenciamentoParaAtualizar = FirstNoTracking(c => c.IdCredenciamento == viewModel.IdCredenciamento);
            if (credenciamentoParaAtualizar == null)
            {
                throw new NotFoundException("credenciamento_nao_encontrado");
            }

            ValidaCredenciemento(viewModel, true);

            credenciamentoParaAtualizar.Estabelecimento = viewModel.Estabelecimento;
            credenciamentoParaAtualizar.Email = viewModel.Email;
            credenciamentoParaAtualizar.Telefone = UtilBase.FiltrarDigitos(viewModel.Telefone);
            credenciamentoParaAtualizar.IdCategoria = viewModel.IdCategoria;
            credenciamentoParaAtualizar.Cep = UtilBase.FiltrarDigitos(viewModel.Cep);
            credenciamentoParaAtualizar.IdCidade = viewModel.IdCidade;
            credenciamentoParaAtualizar.Bairro = viewModel.Bairro;
            credenciamentoParaAtualizar.Rua = viewModel.Rua;
            credenciamentoParaAtualizar.Numero = viewModel.Numero;
            credenciamentoParaAtualizar.Complemento = viewModel.Complemento;
            credenciamentoParaAtualizar.PercentualCashback = viewModel.PercentualCashback;
            credenciamentoParaAtualizar.Latitude = viewModel.Latitude;
            credenciamentoParaAtualizar.Longitude = viewModel.Longitude;
            credenciamentoParaAtualizar.LoginPatrocinador = viewModel.LoginPatrocinador;
            credenciamentoParaAtualizar.IdEcossistema = viewModel.IdEcossistema;
            credenciamentoParaAtualizar.AceitaPgtoComSaldo = viewModel.AceitaPgtoComSaldo;
            credenciamentoParaAtualizar.ScanGo = viewModel.ScanGo;
            credenciamentoParaAtualizar.Cnpj = UtilBase.FiltrarDigitos(viewModel.Cnpj);
            credenciamentoParaAtualizar.IdCategoria = viewModel.IdCategoria;
            credenciamentoParaAtualizar.CelularContato = viewModel.CelularContato;
            credenciamentoParaAtualizar.LogoUrl = string.IsNullOrEmpty(viewModel.ImageBase64) ? credenciamentoParaAtualizar.LogoUrl : SalvaLogo(viewModel.ImageBase64, credenciamentoParaAtualizar.NomeResponsavel);

            var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == credenciamentoParaAtualizar.IdUsuario);
            usuario.Celular = viewModel.CelularContato;
            usuario.Email = viewModel.Email;
            usuario.Nome = viewModel.NomeResponsavel;
            usuario.Login = viewModel.LoginResponsavel;

            _usuarioNegocio.Update(usuario);
            Update(credenciamentoParaAtualizar);
        }

        private string IdCripto(long id, Guid idPai)
        {
            var strId = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{id}_{idPai}"));
            return strId;
            //var bytes = Encoding.ASCII.GetBytes(Id.ToString());
            //return Convert.ToBase64String(bytes);
        }

        public void EnviarEmailCredenciamento(long IdCredenciamento)
        {
            var credenciamento = First(c => c.IdCredenciamento == IdCredenciamento);
            if (credenciamento == null)
            {
                throw new NotFoundException("credenciamento_nao_encontrado");
            }

            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = credenciamento.Estabelecimento,
                Subject = "BigCash - Confirmação de email",
                To = credenciamento.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
            if (String.IsNullOrEmpty(rootSite))
            {
                _cache.SetItem(CacheKeys.RootSite, _configNegocio.BuscarPelaChave("URL_BASE").Valor);
                rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
            }

            // TODO: verificar este token mais tarde
            var link = rootSite + _appSettings.RootSiteConfirmEmail + IdCripto(credenciamento.IdCredenciamento, credenciamento.IdUsuarioPai);

            var body = new Dictionary<string, string>
            {
                { "#NOMEESTABELECIMENTO#", objectEmail.DestinationName },
                { "#URL", link}
            };

            _ = new EmailUtilitis().EnviarEmail(body, _appSettings.ConfirmarEmail, _appSettings.TemplatePai, objectEmail);
        }

        private string SalvaLogo(string imageBase64, string idUsuario)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var image = Convert.FromBase64String(imageBase64);
                using (var ms = new MemoryStream(image))
                {
                    var img = Image.FromStream(ms);

                    //if (img.Width > 320 || img.Height > 320)
                    //{
                    //    throw new PadraoException("logo_resolucao_maxima");
                    //}
                }

                var logoUrl = AzureStorage.CreateBlob(
                        imageBase64,
                        new Guid(),
                        _appSettings.StorageAccountConnectionString,
                        "imagens-credenciamento",
                        idUsuario + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                        true
                    ).Result;

                return logoUrl;
            }

            return string.Empty;
        }

        /// <summary>
        /// Verifica se os dados existem e se não estão em uso
        /// </summary>
        /// <param name="viewModel"></param>
        private void ValidaCredenciemento(CredenciamentoViewModel viewModel, bool isUpdate = false)
        {
            if (viewModel.PercentualCashback <= 0)
            {
                throw new PadraoException("cashback_percentual_minimo");
            }

            var patrocinador = _usuarioNegocio.FirstNoTracking(f => f.Login == viewModel.LoginPatrocinador && (isUpdate || f.Ativo));
            if (patrocinador == null)
                throw new NotFoundException("patrocinador_nao_encontrado");

            var cidade = _cidadeNegocio.FirstNoTracking(c => c.IdCidade == viewModel.IdCidade);
            if (cidade == null)
                throw new NotFoundException("cidade_nao_encontrada");

            var categoria = _categoriaNegocio.FirstNoTracking(c => c.IdCategoria == viewModel.IdCategoria);
            if (categoria == null)
                throw new NotFoundException("categoria_nao_encontrada");

            if (viewModel.LoginResponsavel.TemCaracterEspecial())
                throw new PadraoException("login_caracteres_invalidos");

            var usuarioLogin = _usuarioNegocio.FirstNoTracking(u => u.Login == viewModel.LoginResponsavel);
            if (!isUpdate && usuarioLogin != null)
            {
                throw new PadraoException("login_em_uso");
            }

            usuarioLogin = _usuarioNegocio.FirstNoTracking(u => u.Email == viewModel.Email);
            if (!isUpdate && usuarioLogin != null)
            {
                throw new PadraoException("email_em_uso");
            }

            usuarioLogin = _usuarioNegocio.FirstNoTracking(u => u.Documento == viewModel.Cnpj);
            if (!isUpdate && usuarioLogin != null)
            {
                throw new PadraoException("cpf_cnpj_em_uso");
            }

            usuarioLogin = _usuarioNegocio.FirstNoTracking(u => u.Celular == viewModel.Telefone);
            if (!isUpdate && usuarioLogin != null)
            {
                throw new PadraoException("telefone_em_uso");
            }

            var telefoneEmailCadastradoEmOutroCredenciamento = GetNoTracking(c =>
                c.Telefone == viewModel.Telefone &&
                c.IdCredenciamento != viewModel.IdCredenciamento);
            if (
                !isUpdate &&
                telefoneEmailCadastradoEmOutroCredenciamento != null &&
                telefoneEmailCadastradoEmOutroCredenciamento.Count > 0)
            {
                throw new PadraoException("telefone_em_uso");
            }

            telefoneEmailCadastradoEmOutroCredenciamento = GetNoTracking(c =>
                c.Email == viewModel.Email &&
                c.IdCredenciamento != viewModel.IdCredenciamento);
            if (
                !isUpdate &&
                telefoneEmailCadastradoEmOutroCredenciamento != null &&
                telefoneEmailCadastradoEmOutroCredenciamento.Count > 0)
            {
                throw new PadraoException("email_em_uso");
            }
        }

        public void EditarAnunciante(LojasCredenciadoViewModel editar, Guid IdUsuarioLogado, long IdCredenciamento)
        {

            var dadosAnunciante = _repositorio.GetById(IdCredenciamento);

            var apagarImagem = editar.LogoUrl;

            var logoUrl = dadosAnunciante.LogoUrl;

            if (logoUrl == null)
            {
                logoUrl = "";
            }
            if (string.IsNullOrEmpty(apagarImagem) && logoUrl != "")
            {
                logoUrl = "";
            }



            else if (!string.IsNullOrEmpty(editar.ImageBase64))
            {
                var image = Convert.FromBase64String(editar.ImageBase64);
                using (var ms = new MemoryStream(image))
                {
                    var img = Image.FromStream(ms);

                    if (img.Width != img.Height)
                    {
                        throw new PadraoException("imagem_app_resolucao_maxima");
                    }
                }

                logoUrl = AzureStorage.CreateBlob(
                    editar.ImageBase64,
                    new Guid(),
                    _appSettings.StorageAccountConnectionString,
                    "imagens-credenciamento",
                    "image-credenciamento-" + new Guid().ToString(),
                    true
                ).Result;

            }

            dadosAnunciante.Estabelecimento = editar.Estabelecimento;
            dadosAnunciante.Telefone = editar.Telefone;
            dadosAnunciante.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();
            dadosAnunciante.LogoUrl = logoUrl;
            dadosAnunciante.Bairro = editar.Bairro;
            dadosAnunciante.Rua = editar.Rua;
            dadosAnunciante.Numero = editar.Numero;
            dadosAnunciante.Cep = editar.Cep;
            dadosAnunciante.Complemento = editar.Complemento;
            dadosAnunciante.Latitude = editar.Latitude;
            dadosAnunciante.Longitude = editar.Longitude;
            dadosAnunciante.PercentualCashback = editar.PercentualCashback;


            _repositorio.Update(dadosAnunciante);
            _repositorio.SaveChanges();

        }

        public async Task<(bool status, string message)> Credenciar(NovoCredenciamentoViewModel credenciamento)
        {
            try
            {
                // verificar se o usuário indicador existe
                if (_usuarioNegocio.FirstNoTracking(x => x.Login == credenciamento.Indicador) is null)
                    return (false, "Indicador não existe");

                // verificar login do usuário
                if (_usuarioNegocio.FirstNoTracking(x => x.Login == credenciamento.CPFResponsavel) is not null)
                    return (false, "Login indisponível");

                // verificar e-mail do usuário
                if (_usuarioNegocio.FirstNoTracking(x => x.Email == credenciamento.EmailResponsavel) is not null)
                    return (false, "E-mail já cadastrado em outra conta");

                // verificar cpf do usuário
                if (_usuarioNegocio.FirstNoTracking(x => x.Documento == credenciamento.CPFResponsavel) is not null)
                    return (false, "CPF do responsável já cadastrado em outra conta");

                // verificar telefone do usuário
                if (_usuarioNegocio.FirstNoTracking(x => x.Celular == credenciamento.TelefoneResponsavel) is not null)
                    return (false, "Telefone do responsável já cadastrado em outra conta");

                // verificar CNPJ/CPF do estabelecimento
                if (_repositorio.GetAllNoTracking().AsEnumerable().FirstOrDefault(x => x.Cnpj == credenciamento.CNPJ) is not null)
                {
                    if (credenciamento.CNPJ.NormalizeString().Length == 11)
                        return (false, "CPF já cadastrado em outro estabelecimento");
                    else
                        return (false, "CNPJ já cadastrado em outro estabelecimento");
                }

                credenciamento.Logomarca = SalvaLogo(credenciamento.Logomarca[(credenciamento.Logomarca.IndexOf(',') + 1)..], Guid.NewGuid().ToString());

                return await _repositorio.Credenciar(credenciamento);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                return (false, message);
            }
        }
    }
}
