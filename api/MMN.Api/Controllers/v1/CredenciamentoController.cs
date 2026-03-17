using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Api.Services;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Results;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredenciamentoController : LoggedControllerBase
    {
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private AppSettings _appSettings;
        private readonly ICache _cache;
        private readonly ITokenUtil _token;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly IAlteracaoPerfilNegocio _alteracaoPerfilNegocio;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly ICidadeNegocio _cidadeNegocio;
        private readonly ICupomCashbackNegocio _cupomCashbackNegocio;
        private readonly ICuponCashbackPedidoRepositorio _cuponCashbackPedidoRepositorio;
        private readonly ICategoriaNegocio _categoria;
        private readonly IBotConversaService _botConversa;
        public CredenciamentoController(
            ICredenciamentoNegocio credenciamentoNegocio,
            IUsuarioNegocio usuarioNegocio,
            IOptions<AppSettings> appSettings,
            ICache cache,
            ITokenUtil token,
            IConfiguracaoNegocio configNegocio,
            IAlteracaoPerfilNegocio alteracaoPerfilNegocio,
            IPedidoNegocio pedidoNegocio,
            ICidadeNegocio cidadeNegocio,
            ILancamentoNegocio lancamentoNegocio,
            ICupomCashbackNegocio cupomCashbackNegocio,
            ICuponCashbackPedidoRepositorio cuponCashbackPedidoRepositorio,
            ICategoriaNegocio categoria,
            IBotConversaService botConversa)
        {
            _credenciamentoNegocio = credenciamentoNegocio;
            _usuarioNegocio = usuarioNegocio;
            _appSettings = appSettings.Value;
            _cache = cache;
            _token = token;
            _configNegocio = configNegocio;
            _alteracaoPerfilNegocio = alteracaoPerfilNegocio;
            _pedidoNegocio = pedidoNegocio;
            _cidadeNegocio = cidadeNegocio;
            _lancamentoNegocio = lancamentoNegocio;
            _cupomCashbackNegocio = cupomCashbackNegocio;
            _cuponCashbackPedidoRepositorio = cuponCashbackPedidoRepositorio;
            _categoria = categoria;
            _botConversa = botConversa;
        }

        [HttpGet]
        [Route("GetStatus")]
        public IActionResult GetStatus()
        {
            var lista = new Dictionary<string, int>
                {
                    { StatusCredenciamento.Aprovado.GetDescription(), (int)StatusCredenciamento.Aprovado },
                    { StatusCredenciamento.Reprovado.GetDescription(), (int)StatusCredenciamento.Reprovado },
                    { StatusCredenciamento.Pendente.GetDescription(), (int)StatusCredenciamento.Pendente },
                    //{ StatusCredenciamento.PreCadastro.GetDescription(), (int)StatusCredenciamento.PreCadastro }
                };

            return Ok(lista);
        }

        [HttpGet]
        [Route("GetCredenciamentos")]
        public IActionResult GetCredenciamentos()
        {
            var credenciamentos = _credenciamentoNegocio.Get(c => c.Status == StatusCredenciamento.Aprovado, "Cidade.Estado").Select(c => new
            {
                c.LogoUrl,
                c.Estabelecimento,
                c.PercentualCashback,
                c.Latitude,
                c.Longitude,
                estado = c.CidadeViewModel.Estado.Nome,
                cidade = c.CidadeViewModel.Nome,
                c.Bairro,
                c.Rua,
                c.Numero,
                c.Complemento
            });
            return Ok(credenciamentos);
        }

        [HttpPost, Authorize, Route("FiltrarCredenciamentosApp")]
        public IActionResult FiltrarCredenciamentosApp(FiltroCredenciamentoApp filtroCredenciamento)
        {
            var credenciamentos = _credenciamentoNegocio.Get(c => c.Status == StatusCredenciamento.Aprovado && !string.IsNullOrEmpty(c.Latitude) && !string.IsNullOrEmpty(c.Longitude) && c.IdUsuario != IdUsuarioLogado &&
                (!filtroCredenciamento.IdCategoria.HasValue || c.IdCategoria == filtroCredenciamento.IdCategoria), "Cidade.Estado", "Usuario", "UsuarioPai", "Categoria").Select(c => new
                {
                    c.IdCredenciamento,
                    c.Latitude,
                    c.Longitude,
                    c.Estabelecimento,
                    c.LogoUrl,
                    c.IdCategoria,
                    CategoriaNome = c.CategoriaViewModel.Nome,
                    Comerciante = c.UsuarioViewModel.Login,
                    c.Cep,
                    Cidade = c.CidadeViewModel.Nome,
                    Estado = c.CidadeViewModel.Estado.Nome,
                    c.Bairro,
                    c.Rua,
                    c.Numero,
                    c.Telefone,
                    c.Email,
                    c.LoginPatrocinador,
                    c.DataAtivacao,
                    c.DataCadastro,
                    c.NomeResponsavel,
                    c.PercentualCashback,
                    c.CelularContato
                }).ToList();

            return Ok(credenciamentos);
        }

        [HttpPost]
        [Route("FiltrarCredenciamentos")]
        [Authorize]
        public IActionResult FiltrarCredenciamento(FiltroCredenciamento filtroCredenciamento)
        {
            return Ok(_credenciamentoNegocio.FiltrarCredenciamento(filtroCredenciamento));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(CredenciamentoViewModel viewModel)
        {
            var viewModelValidator = new CredenciamentoViewModelUpdateValidator();
            var result = await viewModelValidator.ValidateAsync(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            viewModel.DataAtualizacao = DateTime.Now.HorarioBrasilia();
            _credenciamentoNegocio.Atualizar(viewModel);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(CredenciamentoViewModel viewModel)
        {
            var viewModelValidator = new CredenciamentoViewModelUpdateStatusValidator();
            var result = await viewModelValidator.ValidateAsync(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdCredenciamento == viewModel.IdCredenciamento);
            if (credenciamento == null)
            {
                throw new NotFoundException("credenciamento_nao_encontrado");
            }

            if (viewModel.PercentualCashback > 0)
                credenciamento.PercentualCashback = viewModel.PercentualCashback;

            credenciamento.Status = viewModel.Status;
            if (credenciamento.Status == StatusCredenciamento.Reprovado)
                credenciamento.MotivoRecusa = viewModel.MotivoRecusa;
            //else if (credenciamento.Status == StatusCredenciamento.Aprovado)
            //{
            //    credenciamento.LogoUrl = await new Azure().CreateBlob(viewModel.ImageBase64, credenciamento.IdUsuario.Value, _appSettings.StorageAccountConnectionString);
            //    credenciamento.Latitude = viewModel.Latitude;
            //    credenciamento.Longitude = viewModel.Longitude;
            //}

            if (credenciamento.Status == StatusCredenciamento.Aprovado)
            {
                credenciamento.DataAtivacao = DateTime.Now.HorarioBrasilia();
                var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == credenciamento.IdUsuario);

                usuario.EmailConfirmado = true;
                _usuarioNegocio.Update(usuario);

                await EnviarEmailAprovacao(usuario.IdUsuario, "Quanta Shop - Seu cadastro foi aprovado!");
            }

            credenciamento.DataAtualizacao = DateTime.Now.HorarioBrasilia();
            _credenciamentoNegocio.Update(credenciamento);
            return Ok();
        }

        //
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("AlterarPercentualCashback")]
        public async Task<IActionResult> AlterarPercentualCashback(CredenciamentoViewModel viewModel)
        {
            var viewModelValidator = new CredenciamentoViewModelUpdateStatusValidator();
            var result = await viewModelValidator.ValidateAsync(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdCredenciamento == viewModel.IdCredenciamento);
            if (credenciamento == null)
            {
                throw new NotFoundException("credenciamento_nao_encontrado");
            }

            if (viewModel.PercentualCashback > 0)
                credenciamento.PercentualCashback = viewModel.PercentualCashback;

            credenciamento.DataAtualizacao = DateTime.Now.HorarioBrasilia();
            _credenciamentoNegocio.Update(credenciamento);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ReenviarEmail/{idCredenciamento}")]
        public IActionResult ReenviarEmail(long idCredenciamento)
        {
            var credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdCredenciamento.ToString() == idCredenciamento.ToString());

            EnviarEmailConfirmacao((Guid)credenciamento.IdUsuario, "Quanta Shop - Confirmação de email");

            //_credenciamentoNegocio.EnviarEmailCredenciamento(idCredenciamento);
            //EnviarEmailConfirmacao(idCredenciamento.ToString()), "Quanta Shop - Confirmação de email");
            return Ok();
        }

        private Task<bool> EnviarEmailConfirmacao(Guid idUsuario, string titulo)
        {
            var usuario = _usuarioNegocio.GetById(idUsuario);
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = titulo,
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var webToken = _token.ConstruirToken(usuario);
            var rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));

            if (string.IsNullOrEmpty(rootSite))
            {
                _cache.SetItem(CacheKeys.RootSite, _configNegocio.BuscarPelaChave("URL_BASE").Valor);
                rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
            }

            var link = rootSite + _appSettings.RootSiteConfirmEmail + webToken;

            var body = new Dictionary<string, string>
            {
                { "{{ name }}", objectEmail.DestinationName },
                { "{{ confirmation_link }}", link}
            };

            return new EmailUtilitis().EnviarEmail(body, _appSettings.ConfirmarEmail, null, objectEmail);
        }

        private Task<bool> EnviarEmailNovoCredenciamentoParaAprovar(string razaoSocial, string nomeResponsavel, string cashback, string telefone, int idCategoria)
        {
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = "Administrador",
                Subject = "Temos um novo cadastro de solicitação de credenciamento 😎",
                To = "contato@quantabank.com.br",
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var categoria = _categoria.GetById(idCategoria);

            if (telefone.Length == 10)
                telefone = long.Parse(telefone).ToString(@"(00) 0000-0000");

            if (telefone.Length == 11)
                telefone = long.Parse(telefone).ToString(@"(00) 00000-0000");

            var body = new Dictionary<string, string>
            {
                { "{{ name }}", razaoSocial.ToUpper() },
                { "{{ cashback }}", cashback },
                { "{{ phone }}", telefone },
                { "{{ category }}", categoria.Nome.ToUpper() },
                { "{{ responsible }}", nomeResponsavel}
            };

            return new EmailUtilitis().EnviarEmail(body, _appSettings.NovoCredenciamentoParaAprovar, null, objectEmail);
        }

        private Task<bool> EnviarEmailNovoCredenciamento(string emailIndicador, string nomeIndicador, string razaoSocial, string nomeResponsavel, string cashback, string telefone, int idCategoria)
        {
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = nomeIndicador,
                Subject = "Você tem um novo cadastro de credenciamento 😎",
                To = emailIndicador,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var categoria = _categoria.GetById(idCategoria);

            if (telefone.Length == 10)
                telefone = long.Parse(telefone).ToString(@"(00) 0000-0000");

            if (telefone.Length == 11)
                telefone = long.Parse(telefone).ToString(@"(00) 00000-0000");

            var body = new Dictionary<string, string>
            {
                { "{{ invitor }}", nomeIndicador.ToUpper() },
                { "{{ name }}", razaoSocial.ToUpper() },
                { "{{ cashback }}", cashback },
                { "{{ phone }}", telefone },
                { "{{ category }}", categoria.Nome.ToUpper() },
                { "{{ responsible }}", nomeResponsavel.ToUpper()}
            };

            return new EmailUtilitis().EnviarEmail(body, _appSettings.NovoCredenciamento, null, objectEmail);
        }

        private Task<bool> EnviarEmailAprovacao(Guid idUsuario, string titulo)
        {
            var usuario = _usuarioNegocio.GetById(idUsuario);
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = titulo,
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var webToken = _token.ConstruirToken(usuario);
            var rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));

            if (string.IsNullOrEmpty(rootSite))
            {
                _cache.SetItem(CacheKeys.RootSite, _configNegocio.BuscarPelaChave("URL_BASE").Valor);
                rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
            }

            var link = rootSite + _appSettings.RootSiteConfirmEmail + webToken;

            var body = new Dictionary<string, string>
            {
                { "#NOMECLIENTE#", objectEmail.DestinationName },
            };

            return new EmailUtilitis().EnviarEmail(body, _appSettings.AprovadoUsuarioCredenciamento, _appSettings.TemplatePai, objectEmail);
        }

        [HttpPost, Authorize, Route("meusCredenciamentos")]
        public IActionResult MeusCredenciamentos(FiltroMeusCredenciamentos viewModel)
        {
            var credenciamentos = _credenciamentoNegocio.Get(c =>
                    c.IdUsuarioPai == IdUsuarioLogado
                    && (string.IsNullOrEmpty(viewModel.Estabelecimento) || c.Estabelecimento.ToLower().Contains(viewModel.Estabelecimento.ToLower()))
                    && (!viewModel.IdCategoria.HasValue || c.IdCategoria == viewModel.IdCategoria)
                    && (!viewModel.IdStatus.HasValue || c.Status == viewModel.IdStatus)
                    , "Usuario", "Categoria", "Cidade").ToList();

            credenciamentos.ForEach(c =>
            {
                c.Pedidos = _pedidoNegocio.Get(x => x.IdUsuarioComerciante == c.IdUsuario);
            });

            var lista = credenciamentos.Select(s => new
            {
                s.IdCredenciamento,
                s.IdUsuario,
                s.Estabelecimento,
                StatusDesc = s.Status.GetDescription(),
                s.Status,
                s.MotivoRecusa,
                Login = s.UsuarioViewModel != null ? s.UsuarioViewModel.Login : "",
                Categoria = s.CategoriaViewModel.Nome,
                Cidade = s.CidadeViewModel.Nome,
                s.Bairro,
                s.Rua,
                s.Numero,
                s.DataCadastro,
                s.DataAtualizacao,
                s.Cnpj,
                DataUltimaVenda = s.Pedidos.Max(p => (DateTime?)p.DataPedido),
                TotalVendas = s.Pedidos.Sum(p => (decimal?)p.ValorPedido)
            });

            return Ok(lista);
        }

        [HttpPost, AllowAnonymous, Route("v2/credenciar")]
        public async Task<IActionResult> CredenciarV2(NovoCredenciamentoViewModel credenciamento)
        {
            var (status, message) = await _credenciamentoNegocio.Credenciar(credenciamento);

            if (status)
            {
                var usuario = _usuarioNegocio.GetByLoginOrEmail(credenciamento.CPFResponsavel);
                var usuarioIndicador = _usuarioNegocio.GetByLoginOrEmail(credenciamento.Indicador);
                var percentual = string.Format("{0:P2}", Convert.ToDouble(credenciamento.PercentualCashback) / 100);

                // E-mail de confirmação da criação da conta
                await EnviarEmailConfirmacao(usuario.IdUsuario, "Confirme seu email ✉");

                // Enviar e-mail para os administradores com um novo credeciamento a ser aprovado
                await EnviarEmailNovoCredenciamentoParaAprovar(credenciamento.RazaoSocial, credenciamento.NomeResponsavel, percentual, credenciamento.TelefoneResponsavel, credenciamento.IdCategoria);

                // Enviar e-mail para o indicador
                await EnviarEmailNovoCredenciamento(usuarioIndicador.Email, usuarioIndicador.Nome, credenciamento.RazaoSocial, credenciamento.NomeResponsavel, percentual, credenciamento.TelefoneResponsavel, credenciamento.IdCategoria);

                var phone = "+55" + usuario.Celular;
                var username = usuario.Nome.Split(' ');
                var firstName = username.Length > 0 ? username[0] : string.Empty;
                var lastName = username.Length > 1 ? username[1] : string.Empty;

                if (string.IsNullOrEmpty(firstName))
                    firstName = usuario.Email;

                if (string.IsNullOrEmpty(lastName))
                    lastName = usuario.Celular;

                var subscriber = await _botConversa.CreateSubscriberAsync(phone, firstName, lastName);

                if (subscriber is not null)
                {
                    await _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                    await _botConversa.SendFlowAsync(subscriber.Id, 7109853); //QS Boas Vindas
                    await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email); // Email

                    string documento = Regex.Replace(usuario.Documento, @"\D", ""); // Remove tudo que não é 

                    await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento); // CNPJ
                    await _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}"); // Link_Indicação_QS
                    await _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784190); // Tag PF
                }

                return Ok(new { message = "Formulário recebido com sucesso" });
            }
            else
                return BadRequest(new { message });
        }

        [HttpPost, Obsolete($"Take {nameof(CredenciarV2)} instead", false)]
        [AllowAnonymous]
        public async Task<IActionResult> Credenciar(CredenciarNovoUsuarioViewModel viewModel)
        {
            // Verifica se os campos foram preenchidos corretamente
            var viewModelValidator = new CredenciarNovoUsuarioViewModelValidator();
            var result = await viewModelValidator.ValidateAsync(viewModel);

            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(e => new PadraoException(e.ErrorMessage)));

            var usuario = _credenciamentoNegocio.CredenciarNovoUsuario(viewModel);

            await EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email");

            var phone = "+55" + usuario.Celular;
            var username = usuario.Nome.Split(' ');
            var firstName = username.Length > 0 ? username[0] : string.Empty;
            var lastName = username.Length > 1 ? username[1] : string.Empty;

            if (string.IsNullOrEmpty(firstName))
                firstName = usuario.Email;

            if (string.IsNullOrEmpty(lastName))
                lastName = usuario.Celular;

            var subscriber = await _botConversa.CreateSubscriberAsync(phone, firstName, lastName);

            if (subscriber is not null)
            {
                await _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                await _botConversa.SendFlowAsync(subscriber.Id, 7109853); //QS Boas Vindas
                await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email); // Email

                string documento = Regex.Replace(usuario.Documento, @"\D", ""); // Remove tudo que não é 

                await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento); // CNPJ
                await _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}"); // Link_Indicação_QS
                await _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784190); // Tag PF
            }

            return Ok();
        }

        [HttpPost, Authorize, Route("CadastroLogado")]
        public async Task<IActionResult> CadastroLogado(CredenciarViewModel viewModel)
        {
            var viewModelValidator = new CredenciarViewModelValidator();
            var result = await viewModelValidator.ValidateAsync(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var comerciante = _credenciamentoNegocio.Credenciar(viewModel, IdUsuarioLogado);

            return Ok();
        }

        [HttpGet]
        [Route("obterDadosLojaCredenciada/{idCredenciamento}")]
        public IActionResult ObterDadosCredenciamento(long idCredenciamento)
        {
            var urlAndroid = _configNegocio.FirstNoTracking(c => c.Chave == "URL_APP_ANDROID");
            var urlApple = _configNegocio.FirstNoTracking(c => c.Chave == "URL_APP_IOS");

            var credenciamento = _credenciamentoNegocio.First(f => f.IdCredenciamento == idCredenciamento, "Cidade.Estado", "UsuarioPai", "Usuario", "Categoria");
            if (credenciamento != null && credenciamento.UsuarioViewModel.Ativo)
            {
                return Ok(new
                {
                    loginPatrocinador = credenciamento.UsuarioPaiViewModel.Login,
                    nomePatrocinador = credenciamento.UsuarioPaiViewModel.Nome,
                    celularPatrocinador = credenciamento.UsuarioPaiViewModel.Celular,
                    nomeResponsavel = credenciamento.UsuarioViewModel.Nome,
                    celularContato = credenciamento.UsuarioViewModel.Celular,
                    credenciamento.DataCadastro,
                    dataAtualizacao = credenciamento.DataAtualizacao != null ? ((DateTime)credenciamento.DataAtualizacao).ToString("dd/MM/yyyy") : "",
                    dataAtivacao = credenciamento.DataAtivacao != null ? ((DateTime)credenciamento.DataAtivacao).ToString("dd/MM/yyyy") : "",
                    credenciamento.Rua,
                    credenciamento.Numero,
                    credenciamento.Bairro,
                    credenciamento.Cnpj,
                    credenciamento.Cep,
                    credenciamento.Complemento,
                    credenciamento.Telefone,
                    credenciamento.Email,
                    credenciamento.Estabelecimento,
                    credenciamento.LogoUrl,
                    credenciamento.Latitude,
                    credenciamento.Longitude,
                    credenciamento.CidadeViewModel.IdCidade,
                    credenciamento.CidadeViewModel.Estado.IdEstado,
                    credenciamento.CategoriaViewModel.IdCategoria,
                    Cidade = credenciamento.CidadeViewModel.Nome,
                    Estado = credenciamento.CidadeViewModel.Estado.Nome,
                    EstadoUf = credenciamento.CidadeViewModel.Estado.Uf,
                    percentualCashback = credenciamento.PercentualCashback,
                    urlApple = urlApple.Valor,
                    urlAndroid = urlAndroid.Valor
                });
            }

            throw new NotFoundException("credenciamento_nao_encontrado");
        }

        [HttpGet]
        [Authorize]
        [Route("obterDadosCredenciamento")]
        public IActionResult ObterDadosCredenciamento()
        {
            var credenciamento = _credenciamentoNegocio.First(f => f.IdUsuario == IdUsuarioLogado, "Cidade.Estado", "UsuarioPai", "Usuario", "Categoria");
            if (credenciamento != null)
            {
                return Ok(new
                {
                    loginPatrocinador = credenciamento.UsuarioPaiViewModel.Login,
                    nomePatrocinador = credenciamento.UsuarioPaiViewModel.Nome,
                    celularPatrocinador = credenciamento.UsuarioPaiViewModel.Celular,
                    nomeResponsavel = credenciamento.UsuarioViewModel.Nome,
                    celularContato = credenciamento.UsuarioViewModel.Celular,
                    credenciamento.DataCadastro,
                    dataAtualizacao = credenciamento.DataAtualizacao != null ? ((DateTime)credenciamento.DataAtualizacao).ToString("dd/MM/yyyy") : "",
                    dataAtivacao = credenciamento.DataAtivacao != null ? ((DateTime)credenciamento.DataAtivacao).ToString("dd/MM/yyyy") : "",
                    credenciamento.Rua,
                    credenciamento.Numero,
                    credenciamento.Bairro,
                    credenciamento.Cnpj,
                    credenciamento.Cep,
                    credenciamento.Complemento,
                    credenciamento.Telefone,
                    credenciamento.Email,
                    credenciamento.Estabelecimento,
                    credenciamento.LogoUrl,
                    credenciamento.Latitude,
                    credenciamento.Longitude,
                    credenciamento.CidadeViewModel.IdCidade,
                    credenciamento.CidadeViewModel.Estado.IdEstado,
                    credenciamento.CategoriaViewModel.IdCategoria,
                    Cidade = credenciamento.CidadeViewModel.Nome,
                    Estado = credenciamento.CidadeViewModel.Estado.Nome,
                    percentualCashback = credenciamento.PercentualCashback,
                    credenciamento.BreveDescricao,
                    credenciamento.DescricaoCompleta,
                    credenciamento.AceitaPgtoComSaldo,
                    credenciamento.ScanGo
                });
            }

            throw new NotFoundException("credenciamento_nao_encontrado");
        }

        [HttpGet]
        [Authorize]
        [Route("obterDadosCredenciamentoPeloNome")]
        public IActionResult ObterDadosCredenciamentoPeloNome([FromQuery] string nome)
        {
            try
            {
                Array credenciamentos = Array.Empty<object>();

                if (nome is not null && nome.Length >= 3)
                {
                    string normalizedNome = RemoveDiacritics(nome).ToUpper();

                    credenciamentos = _credenciamentoNegocio.Get(x => x.Status == StatusCredenciamento.Aprovado)
                        .AsEnumerable() // Traz os dados para a memória
                        .Where(x => RemoveDiacritics(x.Estabelecimento).ToUpper().Contains(normalizedNome)) // Filtra em memória
                        .Select(x => new
                        {
                            id = x.IdUsuario,
                            x.Cnpj,
                            name = RemoveDiacritics(x.Estabelecimento).ToUpper(),
                            label = $"{RemoveDiacritics(x.Estabelecimento).ToUpper()} - ({x.Cnpj})",
                            percentage = x.PercentualCashback
                        })
                        .OrderBy(x => x.name)
                        .ToArray();
                }

                return Ok(new { data = credenciamentos });

            }
            catch (Exception ex)
            {
                var message = ex.InnerException is null ? ex.Message : ex.InnerException.ToString();

                return StatusCode(500, new { data = Array.Empty<object>(), message });
            }
        }

        [HttpGet]
        [Route("solicitarPerfilEmpreendedor")]
        [Authorize]
        public IActionResult SolicitarPerfilEmpreendedor()
        {
            var alteracaoPerfil = _alteracaoPerfilNegocio.First(f => f.IdUsuario == IdUsuarioLogado);

            if (alteracaoPerfil == null)
            {
                var alteracaoPerfilViewModel = new AlteracaoPerfilViewModel()
                {
                    Aceito = false,
                    DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                    IdUsuario = IdUsuarioLogado
                };

                _alteracaoPerfilNegocio.Insert(alteracaoPerfilViewModel);

                return Ok("Solicitação realizada com sucesso, aguarde até que ela seja aprovada!");
            }

            throw new PadraoException("solicitacao_pendente_existe");
        }

        [HttpGet]
        [Route("verificaPerfil")]
        [Authorize]
        public IActionResult VerificaPerfil()
        {
            var alteracaoPerfil = _alteracaoPerfilNegocio.First(f => f.IdUsuario == IdUsuarioLogado);

            if (alteracaoPerfil == null)
            {
                return Ok(true);
            }

            return Ok(false);
        }


        [Obsolete("Deve ser removido após atualizaçao do app mobile para a venda e compra offline")]
        [HttpPost, Route("EfetuarCompra"), Authorize]
        public async Task<IActionResult> EfetuarCompra(EfetuarCompraViewModel viewModel)
        {
            var viewModelValidator = new EfetuarCompraViewModelValidator();
            var result = viewModelValidator.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            await _pedidoNegocio.EfetuarCompraComerciante(viewModel, IdUsuarioLogado);

            return Ok(new
            {
                message = "Compra efetuada com sucesso.",
                Warning = "Este método está obsoleto e será removido em uma atualização futura."
            });
        }



        [HttpGet, Route("ObterUltimasVendas"), Authorize]
        public IActionResult GetUltimasVendas()
        {
            var vendas = _pedidoNegocio.Get(p =>
                p.Ativo.Value
                && p.IdUsuarioComerciante.Value == IdUsuarioLogado)
                .OrderByDescending(p => p.DataPedido)
                .Take(3)
                .ToList();

            var retorno = new List<object>();
            foreach (var venda in vendas)
            {
                retorno.Add(new
                {
                    dataPedido = venda.DataPedido,
                    forma = ((EnumTipoPagamento)venda.MeioPagamento).GetDescription(),
                    valorPedido = venda.ValorPedido,
                    percentual = venda.PercentualCashback
                });
            }

            return Ok(retorno);
        }

        [HttpPost, Route("GetVendas"), Authorize]
        public IActionResult GetVendas(FiltroVendasCredenciando viewModel)
        {
            var vendas = _pedidoNegocio.Get(p =>
                p.Ativo.Value
                && p.IdUsuarioComerciante.Value == IdUsuarioLogado
                && (!viewModel.tipoPagamento.HasValue || (int)viewModel.tipoPagamento.Value == p.MeioPagamento)
                && (!viewModel.DataInicio.HasValue || viewModel.DataInicio.Value.Date < p.DataPedido)
                && (!viewModel.DataFim.HasValue || viewModel.DataFim.Value > p.DataPedido.Date), "Usuario", "Transacao.Status", "UsuarioComerciante")
                .OrderByDescending(p => p.DataPedido)
                .ToList();


            if (viewModel.Ordenacao.HasValue && viewModel.Asc.HasValue)
            {
                switch (viewModel.Ordenacao)
                {
                    case EnumOrdenacaoVendas.Usuario:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.Usuario.Login).ToList();
                        else vendas = vendas.OrderByDescending(v => v.Usuario.Login).ToList();
                        break;
                    case EnumOrdenacaoVendas.DataVenda:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.DataPedido).ToList();
                        else vendas = vendas.OrderByDescending(v => v.DataPedido).ToList();
                        break;
                    case EnumOrdenacaoVendas.ValorUsuario:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.ValorPago).ToList();
                        else vendas = vendas.OrderByDescending(v => v.ValorPago).ToList();
                        break;
                    case EnumOrdenacaoVendas.FormaPagamento:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.MeioPagamento).ToList();
                        else vendas = vendas.OrderByDescending(v => v.MeioPagamento).ToList();
                        break;
                    case EnumOrdenacaoVendas.ValorRecebido:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.ValorPago - v.ValorPago * (v.PercentualCashback / 100)).ToList();
                        else vendas = vendas.OrderByDescending(v => v.ValorPago - v.ValorPago * (v.PercentualCashback / 100)).ToList();
                        break;
                    case EnumOrdenacaoVendas.ValorCashbackUsuario:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.ValorPago * (v.PercentualCashback / 100)).ToList();
                        else vendas = vendas.OrderByDescending(v => v.ValorPago * (v.PercentualCashback / 100)).ToList();
                        break;
                    case EnumOrdenacaoVendas.PercentualCashback:
                        if (!viewModel.Asc.Value) vendas = vendas.OrderBy(v => v.PercentualCashback).ToList();
                        else vendas = vendas.OrderByDescending(v => v.PercentualCashback).ToList();
                        break;
                }

            }

            var totalPages = (int)Math.Ceiling((double)vendas.Count() / viewModel.QuantidadePorPagina);

            var vendasFiltradas = viewModel.ObterTudo.HasValue && viewModel.ObterTudo.Value ? vendas : vendas
               .Skip(viewModel.QuantidadePorPagina * (viewModel.Pagina - 1))
               .Take(viewModel.QuantidadePorPagina)
               .ToList();


            List<object> retorno = new List<object>();

            foreach (var venda in vendasFiltradas)
            {
                decimal valorPraRede = 0;
                var lancamentosParaRede = _lancamentoNegocio.Get(l => l.IdTransacao == venda.IdTransacao
                                               && (l.IdTipo == 51 || l.IdTipo == 52)
                                               && l.IdUsuario != IdUsuarioLogado
                                               && l.IdUsuario != new Guid("30C0BF78-879A-47C1-9869-D485B8D84E0B"), "LancamentoRetido");

                var cupomCashbackPedido = _cuponCashbackPedidoRepositorio.Get(ccp => ccp.IdPedido == venda.IdPedido).Include(cc => cc.CuponCashback).FirstOrDefault();

                foreach (var lancamento in lancamentosParaRede
                    .Where(w => (venda.UsuarioComerciante == null || w.IdUsuario != venda.UsuarioComerciante.IdUsuarioPai) && w.IdTipo != 52)
                    .ToList()
                )
                {
                    valorPraRede += lancamento.Valor;
                    //valorPraRede += lancamento.LancamentoRetido.Sum(l => l.Valor);
                }

                var valorParaPtrocinador = lancamentosParaRede.Where(w => venda.Usuario.IdUsuarioPai == null || w.IdUsuario == venda.Usuario.IdUsuarioPai).Sum(s => s.Valor);
                var valorEmpreendedor = lancamentosParaRede.Where(w => w.IdTipo == 52).Sum(s => s.Valor);

                retorno.Add(new
                {
                    Usuario = venda.Usuario.Login,
                    venda.ValorPago,
                    venda.IdTransacao,
                    venda.Transacao.Descricao,
                    ValorTotal = venda.ValorPago,
                    ValorRecebido = venda.ValorPago - venda.ValorPago * (venda.PercentualCashback / 100),
                    ValorCashbackUsuario = venda.ValorPago * (venda.PercentualCashback / 100),
                    venda.PercentualCashback,
                    venda.DataPedido,
                    valorEmpreendedor,
                    Status = venda.Transacao.StatusViewModel.Nome,
                    TipoPagamento = ((EnumTipoPagamento)venda.MeioPagamento).GetDescription(),
                    ValorParaRede = valorPraRede,
                    ValorRecebidoPelaBigcash = _lancamentoNegocio.Get(l => l.IdTransacao == venda.IdTransacao && l.IdUsuario == new Guid("30C0BF78-879A-47C1-9869-D485B8D84E0B")).Sum(s => s.Valor),
                    ValorParaPatrocinador = valorParaPtrocinador,
                    cupomCashbackPedido.CuponCashback.ComprovanteCompra,
                    cupomCashbackPedido.CuponCashback.Token
                });
            }

            return Ok(new { totalPages, viewModel.QuantidadePorPagina, viewModel.Pagina, vendasFiltradas = retorno, quantidadeTotal = vendas.Count() });
        }

        [HttpGet, Route("ObterResumoConta")]
        public IActionResult ObterResumoConta()
        {
            var totalVendas = _pedidoNegocio.Get(p => p.Ativo.HasValue && p.Ativo.Value && p.IdUsuarioComerciante == IdUsuarioLogado).Count;

            decimal valorFaturasAbertas = 0;
            decimal valorFaturasPagas = 0;

            var valorTotalFaturas = 0;
            var lancamentosUsuario = _lancamentoNegocio.Get(l => l.Ativo && l.IdStatus != (int)StatusTransacaoEnum.Cancelada && l.IdUsuario == IdUsuarioLogado);

            return Ok(new
            {
                totalVendas,
                valorTotalFaturas,
                valorFaturasAbertas,
                valorFaturasPagas,
                totalEntradas = lancamentosUsuario.Where(l => l.Valor > 0).Sum(s => s.Valor),
                totalSaidas = lancamentosUsuario.Where(l => l.Valor < 0).Sum(s => s.Valor)
            });
        }

        [HttpPut, Route("AtualizarDadosCredenciamento"), Authorize]
        public async Task<IActionResult> AtualizarDadosCredenciamento(CredenciamentoUpdateViewModel viewModel)
        {
            var validator = new CredenciamentoUpdateViewModelValidator();
            var result = await validator.ValidateAsync(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var credenciamento = _credenciamentoNegocio.GetNoTracking(c => c.IdUsuario == IdUsuarioLogado, "Cidade.Estado").FirstOrDefault();
            var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == credenciamento.IdUsuario);

            credenciamento.Latitude = viewModel.Latitude;
            credenciamento.Cnpj = viewModel.Cnpj;
            credenciamento.Longitude = viewModel.Longitude;
            credenciamento.Cep = viewModel.Cep;
            credenciamento.Email = viewModel.Email;
            credenciamento.Bairro = viewModel.Bairro;
            credenciamento.Rua = viewModel.Rua;
            credenciamento.Numero = viewModel.Numero;
            credenciamento.Telefone = viewModel.Telefone;
            credenciamento.Estabelecimento = viewModel.Estabelecimento;
            credenciamento.Complemento = viewModel.Complemento;
            credenciamento.IdCategoria = viewModel.IdCategoria;
            credenciamento.PercentualCashback = Convert.ToDecimal(viewModel.PercentualCashback);
            credenciamento.BreveDescricao = viewModel.BreveDescricao;
            credenciamento.DescricaoCompleta = viewModel.DescricaoCompleta;
            credenciamento.AceitaPgtoComSaldo = viewModel.AceitaPgtoComSaldo;
            credenciamento.ScanGo = viewModel.ScanGo;

            usuario.Nome = viewModel.NomeResponsavel;
            usuario.Celular = viewModel.CelularContato;

            credenciamento.UsuarioViewModel = usuario;
            credenciamento.CelularContato = viewModel.CelularContato;
            credenciamento.NomeResponsavel = viewModel.NomeResponsavel;

            var cidade = _cidadeNegocio.FirstNoTracking(c => c.IdCidade == viewModel.IdCidade, "Estado");

            credenciamento.CidadeViewModel = cidade;
            credenciamento.Estado = cidade.Estado.Nome;

            if (!string.IsNullOrEmpty(viewModel.ImageBase64))
            {
                var image = Convert.FromBase64String(viewModel.ImageBase64);
                using (var ms = new MemoryStream(image))
                {
                    var img = Image.FromStream(ms);

                    if (img.Width > 1024 || img.Height > 1024)
                    {
                        throw new PadraoException("logo_resolucao_maxima");
                    }
                }

                credenciamento.LogoUrl = await AzureStorage.CreateBlob(
                    viewModel.ImageBase64,
                    IdUsuarioLogado,
                    _appSettings.StorageAccountConnectionString,
                    "imagens-credenciamento",
                    IdUsuarioLogado + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                    true);
            }

            credenciamento.DataAtualizacao = DateTime.Now.HorarioBrasilia();
            _credenciamentoNegocio.Update(credenciamento);

            _usuarioNegocio.Update(usuario);

            return Ok(new { url = credenciamento.LogoUrl });
        }

        [HttpPost]
        [Route("RelatorioAnunciantes")]
        public IList<CredenciamentoViewModel> RelatorioCredenciados(FiltroCredenciamento filtro)
        {
            var anunciantes = _credenciamentoNegocio.GetAll().ToList();
            if (filtro.Estabelecimento != null)
            {
                anunciantes = anunciantes.Where(a => a.Estabelecimento.Contains(filtro.Estabelecimento)).ToList();

            }

            return anunciantes;
        }

        [HttpPost]
        [Route("editarCredenciado/{idCredenciamento}")]
        public IActionResult EditarAnunciante(LojasCredenciadoViewModel viewModel, long idCredenciamento)
        {

            _credenciamentoNegocio.EditarAnunciante(viewModel, IdUsuarioLogado, idCredenciamento);
            return Ok();

        }

        [HttpGet]
        [Authorize]
        [Route("obterDadosCredenciamento/{cnpj}")]
        public IActionResult ObterDadosCredenciamentoPorCNPJ(string cnpj)
        {
            var credenciamento = _credenciamentoNegocio.First(f => f.Cnpj == cnpj, "Cidade.Estado", "UsuarioPai", "Usuario", "Categoria");
            if (credenciamento != null)
            {
                return Ok(new
                {
                    loginPatrocinador = credenciamento.UsuarioPaiViewModel.Login,
                    nomePatrocinador = credenciamento.UsuarioPaiViewModel.Nome,
                    celularPatrocinador = credenciamento.UsuarioPaiViewModel.Celular,
                    nomeResponsavel = credenciamento.UsuarioViewModel.Nome,
                    celularContato = credenciamento.UsuarioViewModel.Celular,
                    credenciamento.DataCadastro,
                    dataAtualizacao = credenciamento.DataAtualizacao != null ? ((DateTime)credenciamento.DataAtualizacao).ToString("dd/MM/yyyy") : "",
                    dataAtivacao = credenciamento.DataAtivacao != null ? ((DateTime)credenciamento.DataAtivacao).ToString("dd/MM/yyyy") : "",
                    credenciamento.Rua,
                    credenciamento.Numero,
                    credenciamento.Bairro,
                    credenciamento.Cnpj,
                    credenciamento.Cep,
                    credenciamento.Complemento,
                    credenciamento.Telefone,
                    credenciamento.Email,
                    credenciamento.Estabelecimento,
                    credenciamento.LogoUrl,
                    credenciamento.Latitude,
                    credenciamento.Longitude,
                    credenciamento.CidadeViewModel.IdCidade,
                    credenciamento.CidadeViewModel.Estado.IdEstado,
                    credenciamento.CategoriaViewModel.IdCategoria,
                    Cidade = credenciamento.CidadeViewModel.Nome,
                    Estado = credenciamento.CidadeViewModel.Estado.Nome,
                    percentualCashback = credenciamento.PercentualCashback,
                    idComerciante = credenciamento.IdUsuario
                });
            }

            throw new NotFoundException("credenciamento_nao_encontrado");
        }

        [HttpPost]
        [Authorize]
        [Route("obterCupomCashbackUsuario")]
        public async Task<IActionResult> obterCupomCashbackUsuario(FiltroVendasCredenciando viewModel)
        {
            viewModel.IdCredenciado = IdUsuarioLogado;
            var credenciamento = await _cupomCashbackNegocio.ObterCuponsCompraUsuarioCredenciado(viewModel);

            return Ok(credenciamento);
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}