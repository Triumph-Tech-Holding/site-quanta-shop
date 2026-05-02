using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Google;
using MMN.Integracoes.Oauth2;
using MMN.Integracoes.Oauth2.Models;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Jwt;
using MMN.Util.Model;
using MMN.Util.Util;
using MundiAPI.PCL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class UsuarioNegocio : BaseNegocio<UsuarioViewModel, Usuario>, IUsuarioNegocio
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IAutenticacaoExternaRepositorio _autenticacaoExternaRepositorio;
        private readonly IGrupoNegocio _grupoNegocio;
        private readonly IGraduacaoNegocio _graduacaoNegocio;
        private readonly IProceduresRepositorio _procedures;
        private readonly IMapper _mapper;
        private readonly IConfiguracaoNegocio _configuracaoNegocio;
        private readonly IMensagemNegocio _mensagemNegocio;
        private readonly IProvedorAutenticacaoNegocio _provedorAutenticacaoNegocio;
        private readonly IParceiroNegocio _parceiroNegocio;
        private readonly ICredenciamentoRepositorio _credenciamentoRepositorio;
        private readonly ICache _cache;
        private readonly IRefreshTokenNegocio _refreshTokenNegocio;
        private readonly IUsuarioEnderecoRepositorio _usuarioEnderecoRepositorio;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly TokenManagement _tokenManagement;
        private readonly AppSettings _appSettings;

        public UsuarioNegocio(
            IUsuarioRepositorio repositorio,
            IAutenticacaoExternaRepositorio autenticacaoExternaRepositorio,
            IGrupoNegocio grupoNegocio,
            IMapper mapper,
            IGraduacaoNegocio graduacaoNegocio,
            IProceduresRepositorio procedures,
            IConfiguracaoNegocio configuracaoNegocio,
            IMensagemNegocio mensagemNegocio,
            IProvedorAutenticacaoNegocio provedorAutenticacaoNegocio,
            IParceiroNegocio parceiroNegocio,
            ICredenciamentoRepositorio credenciamentoRepositorio,
            ICache cache,
            TokenValidationParameters tokenValidationParameters,
            IOptions<TokenManagement> tokenManagement,
            IOptions<AppSettings> appSettings,
            IRefreshTokenNegocio refreshTokenNegocio,
            IUsuarioEnderecoRepositorio usuarioEnderecoRepositorio) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _autenticacaoExternaRepositorio = autenticacaoExternaRepositorio;
            _grupoNegocio = grupoNegocio;
            _mapper = mapper;
            _graduacaoNegocio = graduacaoNegocio;
            _procedures = procedures;
            _configuracaoNegocio = configuracaoNegocio;
            _mensagemNegocio = mensagemNegocio;
            _provedorAutenticacaoNegocio = provedorAutenticacaoNegocio;
            _parceiroNegocio = parceiroNegocio;
            _credenciamentoRepositorio = credenciamentoRepositorio;
            _cache = cache;
            _tokenValidationParameters = tokenValidationParameters;
            _tokenManagement = tokenManagement.Value;
            _appSettings = appSettings.Value;
            _refreshTokenNegocio = refreshTokenNegocio;
            _usuarioEnderecoRepositorio = usuarioEnderecoRepositorio;
        }

        public Guid RegistrarAdmin(UsuarioAdminViewModel model)
        {
            try
            {
                //VerificaPadrão Login
                if (model.Login.TemCaracterEspecial())
                {
                    throw new PadraoException("login_caracteres_invalidos");
                }

                //GRUPO E GRADUAÇÃO INICIAL
                var grupoUsuario = _grupoNegocio.GetById(model.IdGrupo);
                var graduacao = _graduacaoNegocio.ObterMenorNivel();
                var usuarioModel = _mapper.Map<Usuario>(model);

                //Verifica login em uso
                var usuarioLogin = _repositorio.GetByLoginOrEmail(model.Login);

                if (usuarioLogin != null)
                {
                    throw new PadraoException("login_em_uso");
                }

                var salt = Hash.Get_SALT();
                usuarioModel.SaltKey = salt;
                usuarioModel.Ativo = true;
                usuarioModel.Cultura = "pt-BR";
                usuarioModel.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
                usuarioModel.EmailConfirmado = false;
                usuarioModel.Bloqueado = false;
                usuarioModel.TentativasIncorretas = 0;
                usuarioModel.IdGraduacao = graduacao.IdGraduacao;
                usuarioModel.PosicaoBinario = 0;
                usuarioModel.IdGrupo = grupoUsuario.IdGrupo;
                usuarioModel.Senha = Hash.Get_HASH_SHA512(model.Senha, model.Email, salt);
                usuarioModel.UrlImg = "https://bigcash.blob.core.windows.net/imagens-credenciamento/6886c534-1f67-4e5f-8273-c37544ed36622024-05-07-15-19-13.jpeg";
                //usuarioModel.UsuarioAdministrativo = true;

                _repositorio.Insert(usuarioModel);
                _repositorio.SaveChanges();

                return usuarioModel.IdUsuario;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Registrar()", ex, "UsuarioNegocio.cs");
                throw;
            }
        }

        public Usuario CadastroPWA(UsuarioCadastroPWAViewModel model)
        {
            model.Documento = UtilBase.FiltrarDigitos(model.Documento);

            var usuario = _mapper.Map<Usuario>(model);

            //Verifica login Pai
            var usuarioPai = _repositorio.GetByLoginOrEmail(model.LoginPatrocinador);

            if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
            {
                throw new PadraoException("patrocinador_nao_encontrado");
            }

            usuario.IdUsuarioPai = usuarioPai.IdUsuario;
            usuario.EmailConfirmado = false;
            usuario.Login = model.Login;
            usuario.PreCadastro = true;

            return Registrar(usuario);
        }

        public Usuario Registrar(UsuarioCadastroViewModel model)
        {
            var usuario = _mapper.Map<Usuario>(model);

            if (model.LoginPatrocinador.IsNullOrEmpty())
            {
                model.LoginPatrocinador = "quantashop";
            }

            // Vamos atribuir o último cadastro realizado na rede que já tenha confirmado seu e-mail
            if (model.LoginPatrocinador == "quantashop")
            {
                var indicador = _repositorio.Get(x => x.EmailConfirmado).OrderByDescending(x => x.DataCadastro).FirstOrDefault();
                model.LoginPatrocinador = indicador.Login;

                usuario.IndicadoPeloQS = true;
            }

            //Verifica login Pai
            var usuarioPai = _repositorio.GetByLoginOrEmail(model.LoginPatrocinador);
            if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
            {
                throw new PadraoException("patrocinador_nao_encontrado");
            }
            //if (usuarioPai.DataReferencia.HasValue == false)
            //{
            //    throw new PadraoException("patrocinador_nao_encontrado");
            //}
            usuario.IdUsuarioPai = usuarioPai.IdUsuario;
            usuario.EmailConfirmado = false;
            usuario.Documento = UtilBase.FiltrarDigitos(usuario.Documento);
            usuario.PreCadastro = true;

            return Registrar(usuario);
        }

        public Usuario RegistrarFacilitado(UsuarioCadastroFacilitadoViewModel model)
        {
            var usuario = new Usuario();
            var estabelecimento = new Credenciamento();
            var usuarioEstabelecimento = new Usuario();
            var usuarioQS = new Usuario();

            //O cadastro será realizado pelo CNPJ do estabelecimento, caso esteja vazio utilizar o cadastro do QS

            if (model.CNPJ != null)
            {
                estabelecimento = _credenciamentoRepositorio.FirstNoTracking(c => c.Cnpj == model.CNPJ);

                if (estabelecimento != null)
                {
                    usuarioEstabelecimento = _repositorio.FirstNoTracking(r => r.IdUsuario == estabelecimento.IdUsuario);
                    
                    if (usuarioEstabelecimento == null || !usuarioEstabelecimento.Ativo || usuarioEstabelecimento.Master)
                    {
                        throw new PadraoException("patrocinador_nao_encontrado");
                    }
                    if (usuarioEstabelecimento.DataReferencia.HasValue == false)
                    {
                        throw new PadraoException("patrocinador_nao_encontrado");
                    }
                    if (estabelecimento.Status != StatusCredenciamento.Aprovado)
                    {
                        throw new PadraoException("comerciante_nao_aprovado");
                    }
                }
                else
                {
                    usuarioQS = _repositorio.FirstNoTracking(r => r.Login == "quantashop");
                }
            }
            else if (!string.IsNullOrEmpty(model.LoginIndicacao))
            {
                usuarioQS = _repositorio.FirstNoTracking(r => r.Login == model.LoginIndicacao);
            }
            else
            {
                usuarioQS = _repositorio.FirstNoTracking(r => r.Login == "quantashop");
            }

            usuario.IdUsuarioPai = model.CNPJ == null || estabelecimento == null ? usuarioQS.IdUsuario : usuarioEstabelecimento.IdUsuario;
            usuario.EmailConfirmado = false;
            usuario.Documento = UtilBase.FiltrarDigitos(model.CPF);
            usuario.Email = model.Email;
            usuario.Celular = UtilBase.FiltrarDigitos(model.Celular);
            usuario.Login = UtilBase.FiltrarDigitos(model.CPF);
            usuario.Senha = model.Senha;
            usuario.Nome = String.Empty;

            var usuarioRegistrado = RegistrarFacilitado(usuario);

            return usuarioRegistrado;
        }

        public async Task<Usuario> RegistrarGoogleAsync(Oauth2CadastroViewModel model)
        {
            try
            {
                var provedorAutenticacao = await _provedorAutenticacaoNegocio
                    .FirstNoTrackingAsync(g =>
                        g.Protocolo == (int)IdentityProviderProtocol.Oauth2 &&
                        g.Provedor == (int)IdentityProvider.Google);
                var autenticacao = new Oauth2Authenticate
                {
                    ApiUrl = provedorAutenticacao.UrlApi,
                    ClientId = provedorAutenticacao.Login,
                    ClientSecret = provedorAutenticacao.Senha,
                    Code = model.Code,
                    RedirectUri = model.RedirectUri,
                    OptionalParameters = JsonConvert
                        .DeserializeObject<Dictionary<string, string>>(provedorAutenticacao.ParametrosLogin)
                };
                var accessToken = await Oauth2.GetAccessTokenAsync<Oauth2AccessToken>(autenticacao);
                var usuarioGoogle = await GoogleApi.ObterIdUsuarioAsync(accessToken.AccessToken);

                model.Documento = UtilBase.FiltrarDigitos(model.Documento);

                var usuario = _mapper.Map<Usuario>(model);

                //Verifica login Pai
                var usuarioPai = _repositorio.GetByLoginOrEmail(model.LoginPatrocinador);
                if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
                {
                    throw new PadraoException("patrocinador_nao_encontrado");
                }
                //if (usuarioPai.DataReferencia.HasValue == false)
                //{
                //    throw new PadraoException("patrocinador_nao_encontrado");
                //}

                usuario.IdUsuarioPai = usuarioPai.IdUsuario;
                usuario.Email = usuarioGoogle.Email;
                usuario.EmailConfirmado = true;
                usuario.Documento = model.Documento;
                usuario.Senha = string.IsNullOrEmpty(model.Senha) ? "" : model.Senha;

                var autenticacaoExterna = new AutenticacaoExterna
                {
                    IdExterno = usuarioGoogle.Id,
                    Ativo = true,
                    IdProvedorAutenticacao = provedorAutenticacao.IdProvedorAutenticacao
                };

                return Registrar(usuario, autenticacaoExterna);
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Registrar()", ex, "UsuarioNegocio.cs");
                throw;
            }
        }

        public async Task<Usuario> RegistrarGoogleCredentialAsync(Oauth2CredentialCadastroViewModel model)
        {
            try
            {
                string googleId;
                string email;

                using (var client = new HttpClient())
                {
                    var tokenInfoUrl = $"https://oauth2.googleapis.com/tokeninfo?id_token={Uri.EscapeDataString(model.Credential)}";
                    var response = await client.GetAsync(tokenInfoUrl);
                    if (!response.IsSuccessStatusCode)
                        throw new UnauthorizedException("login_incorreto");

                    var json = await response.Content.ReadAsStringAsync();
                    var tokenInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (tokenInfo == null ||
                        !tokenInfo.TryGetValue("sub", out googleId) ||
                        !tokenInfo.TryGetValue("email", out email) ||
                        string.IsNullOrEmpty(googleId) ||
                        string.IsNullOrEmpty(email))
                        throw new UnauthorizedException("login_incorreto");

                    var expectedClientId = _appSettings.GoogleClientId;
                    if (!string.IsNullOrEmpty(expectedClientId) &&
                        (!tokenInfo.TryGetValue("aud", out var aud) || aud != expectedClientId))
                        throw new UnauthorizedException("login_incorreto");
                }

                model.Documento = UtilBase.FiltrarDigitos(model.Documento);

                var usuario = _mapper.Map<Usuario>(model);

                var usuarioPai = _repositorio.GetByLoginOrEmail(model.LoginPatrocinador);
                if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
                    throw new PadraoException("patrocinador_nao_encontrado");

                usuario.IdUsuarioPai = usuarioPai.IdUsuario;
                usuario.Email = email;
                usuario.EmailConfirmado = true;
                usuario.Documento = model.Documento;
                usuario.Senha = string.IsNullOrEmpty(model.Senha) ? "" : model.Senha;

                var provedorAutenticacao = await _provedorAutenticacaoNegocio
                    .FirstNoTrackingAsync(g =>
                        g.Protocolo == (int)IdentityProviderProtocol.Oauth2 &&
                        g.Provedor == (int)IdentityProvider.Google);

                var autenticacaoExterna = new AutenticacaoExterna
                {
                    IdExterno = googleId,
                    Ativo = true,
                    IdProvedorAutenticacao = provedorAutenticacao.IdProvedorAutenticacao
                };

                return Registrar(usuario, autenticacaoExterna);
            }
            catch (Exception ex)
            {
                LogHelper.LogException("RegistrarGoogleCredentialAsync()", ex, "UsuarioNegocio.cs");
                throw;
            }
        }

        private Usuario Registrar(Usuario usuario, AutenticacaoExterna autenticacaoExterna = null)
        {
            try
            {
                //VerificaPadrão Login
                if (usuario.Login.TemCaracterEspecial())
                {
                    throw new PadraoException("login_caracteres_invalidos");
                }

                //GRUPO E GRADUAÇÃO INICIAL
                var grupoUsuario = _grupoNegocio.GetByName("Usuario");
                var graduacao = _graduacaoNegocio.ObterMenorNivel();

                //Verifica login em uso
                var usuarioLogin = _repositorio.GetByLoginOrEmail(usuario.Login);

                if (usuarioLogin != null)
                {
                    throw new PadraoException("login_em_uso");
                }

                usuarioLogin = _repositorio.GetByLoginOrEmail(usuario.Email);
                if (usuarioLogin != null)
                {
                    throw new PadraoException("email_em_uso");
                }

                usuario.Celular = Regex.Replace(usuario.Celular, @"[^\d]", "");

                usuarioLogin = _mapper.Map<Usuario>(FirstNoTracking(u => u.Celular == usuario.Celular));

                if (usuarioLogin != null)
                {
                    throw new PadraoException("telefone_em_uso");
                }

                usuarioLogin = _mapper.Map<Usuario>(FirstNoTracking(u => u.Documento == usuario.Documento));

                if (usuarioLogin != null)
                {
                    throw new PadraoException("cpf_cnpj_em_uso");
                }

                var salt = Hash.Get_SALT();
                usuario.SaltKey = salt;
                usuario.Ativo = true;
                usuario.Cultura = "pt-BR";
                usuario.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
                usuario.Bloqueado = false;
                usuario.TentativasIncorretas = 0;
                usuario.IdGraduacao = graduacao.IdGraduacao;
                usuario.PosicaoBinario = 0;
                //usuarioModel.PosicaoBinarioPai = usuarioPai.PosicaoBinario.GetValueOrDefault();
                usuario.IdGrupo = grupoUsuario.IdGrupo;
                usuario.Senha = Hash.Get_HASH_SHA512(usuario.Senha, usuario.Email, salt);
                usuario.UrlImg = "https://bigcash.blob.core.windows.net/imagens-credenciamento/6886c534-1f67-4e5f-8273-c37544ed36622024-05-07-15-19-13.jpeg";
                usuario.DataReferencia = DateTime.UtcNow.HorarioBrasilia();
                //usuarioModel.UsuarioAdministrativo = false;

                _repositorio.Create(usuario, autenticacaoExterna);
                _repositorio.SaveChanges();


                var mensagemParaPatrocinador = $"{usuario.Login} se cadastrou na sua rede";

                _mensagemNegocio.EnviarNotificacao(usuario.IdUsuarioPai.Value, "Novo cadastro", mensagemParaPatrocinador);

                _procedures.sp_InsertQuantaAmizade(usuario.IdUsuario);
                _procedures.sp_ObjetivoIndicacao(usuario.IdUsuario);

                return usuario;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Registrar()", ex, "UsuarioNegocio.cs");
                throw;
            }
        }

        private Usuario RegistrarFacilitado(Usuario usuario, AutenticacaoExterna autenticacaoExterna = null)
        {
            try
            {
                //VerificaPadrão Login
                if (usuario.Login.TemCaracterEspecial())
                {
                    throw new PadraoException("login_em_uso");
                }

                //GRUPO E GRADUAÇÃO INICIAL
                var grupoUsuario = _grupoNegocio.GetByName("Usuario");
                var graduacao = _graduacaoNegocio.ObterMenorNivel();

                //Verifica login em uso
                var usuarioLogin = _repositorio.GetByLoginOrEmail(usuario.Login);

                if (usuarioLogin != null)
                {
                    throw new PadraoException("login_em_uso");
                }

                usuarioLogin = _repositorio.GetByLoginOrEmail(usuario.Email);
                if (usuarioLogin != null)
                {
                    throw new PadraoException("email_em_uso");
                }

                usuarioLogin = _mapper.Map<Usuario>(FirstNoTracking(u => u.Celular == usuario.Celular));
                if (usuarioLogin != null)
                {
                    throw new PadraoException("telefone_em_uso");
                }

                usuarioLogin = _mapper.Map<Usuario>(FirstNoTracking(u => u.Documento == usuario.Documento));
                if (usuarioLogin != null)
                {
                    throw new PadraoException("cpf_cnpj_em_uso");
                }

                var salt = Hash.Get_SALT();
                usuario.SaltKey = salt;
                usuario.Ativo = true;
                usuario.Cultura = "pt-BR";
                usuario.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
                usuario.Bloqueado = false;
                usuario.TentativasIncorretas = 0;
                usuario.IdGraduacao = graduacao.IdGraduacao;
                usuario.PosicaoBinario = 0;
                usuario.IdGrupo = grupoUsuario.IdGrupo;
                usuario.Senha = Hash.Get_HASH_SHA512(usuario.Senha, usuario.Email, salt);
                usuario.UrlImg = "https://bigcash.blob.core.windows.net/imagens-credenciamento/6886c534-1f67-4e5f-8273-c37544ed36622024-05-07-15-19-13.jpeg";
                usuario.DataReferencia = DateTime.UtcNow.HorarioBrasilia();
                usuario.PreCadastro = true;

                _repositorio.Create(usuario, autenticacaoExterna);
                _repositorio.SaveChanges();


                var mensagemParaPatrocinador = $"{usuario.Login} se cadastrou na sua rede";

                _mensagemNegocio.EnviarNotificacao(usuario.IdUsuarioPai.Value, "Novo cadastro", mensagemParaPatrocinador);

                return usuario;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Registrar()", ex, "UsuarioNegocio.cs");
                throw;
            }
        }

        public UsuarioViewModel Autenticacao(string login, string senha, out Parceiro parceiro, bool verificarSenha = true)
        {
            parceiro = null;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                throw new UnauthorizedException("login_incorreto");
            }

            var usuario = _repositorio
                .Get(g => g.Ativo && (g.Login == login || g.Email == login))
                //.GetAll(/*u => (u.Login == login || u.Email == login) && u.Ativo*/)
                .Include(g => g.Grupo)
                .Include(g => g.UsuarioEndereco)
                .Include(g => g.Credenciamento)
                .FirstOrDefault();

            return Autenticacao(usuario, senha, out parceiro, verificarSenha);
        }

        public UsuarioViewModel AutenticacaoGoogle(string code, string redirectUri, out Parceiro parceiro)
        {
            parceiro = null;

            if (string.IsNullOrEmpty(code))
            {
                throw new UnauthorizedException("login_incorreto");
            }

            var provedorAutenticacao = _provedorAutenticacaoNegocio
                    .FirstNoTrackingAsync(g =>
                        g.Protocolo == (int)IdentityProviderProtocol.Oauth2 &&
                        g.Provedor == (int)IdentityProvider.Google).Result;
            var autenticacao = new Oauth2Authenticate
            {
                ApiUrl = provedorAutenticacao.UrlApi,
                ClientId = provedorAutenticacao.Login,
                ClientSecret = provedorAutenticacao.Senha,
                Code = code,
                RedirectUri = redirectUri,
                OptionalParameters = JsonConvert
                    .DeserializeObject<Dictionary<string, string>>(provedorAutenticacao.ParametrosLogin)
            };

            var accessToken = Oauth2.GetAccessTokenAsync<Oauth2AccessToken>(autenticacao).Result;
            var usuarioGoogle = GoogleApi.ObterIdUsuarioAsync(accessToken.AccessToken).Result;


            if (string.IsNullOrEmpty(usuarioGoogle.Id))
            {
                throw new UnauthorizedException("login_incorreto");
            }

            var usuario = _autenticacaoExternaRepositorio
                .Get(u => u.IdExterno.Equals(usuarioGoogle.Id) && u.Ativo)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.Grupo)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.UsuarioEndereco)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.Credenciamento)
                .Select(s => s.Usuario)
                .FirstOrDefault();

            if (usuario == null)
            {
                usuario =
                _repositorio.Get(u => u.Email.Equals(usuarioGoogle.Email) && u.Ativo)
                  .Include(t => t.Grupo)
                  .Include(t => t.UsuarioEndereco)
                  .Include(t => t.Credenciamentos)
                  .SingleOrDefault();
                if (usuario != null)
                {
                    var autenticacaoExterna = new AutenticacaoExterna
                    {
                        IdExterno = usuarioGoogle.Id,
                        Ativo = true,
                        IdProvedorAutenticacao = provedorAutenticacao.IdProvedorAutenticacao,
                        IdUsuario = usuario.IdUsuario
                    };
                    _autenticacaoExternaRepositorio.Insert(autenticacaoExterna);
                    _autenticacaoExternaRepositorio.SaveChanges();
                }
                else
                {
                    throw new UnauthorizedException("usuario_nao_encontrado");
                }

            }



            return Autenticacao(usuario, null, out parceiro, false);
        }

        public async Task<(UsuarioViewModel usuario, Parceiro parceiro)> AutenticacaoGoogleCredentialAsync(string credential)
        {
            if (string.IsNullOrEmpty(credential))
                throw new UnauthorizedException("login_incorreto");

            string googleId;
            string email;

            using (var client = new HttpClient())
            {
                var tokenInfoUrl = $"https://oauth2.googleapis.com/tokeninfo?id_token={Uri.EscapeDataString(credential)}";
                var response = await client.GetAsync(tokenInfoUrl);
                if (!response.IsSuccessStatusCode)
                    throw new UnauthorizedException("login_incorreto");

                var json = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (tokenInfo == null ||
                    !tokenInfo.TryGetValue("sub", out googleId) ||
                    !tokenInfo.TryGetValue("email", out email) ||
                    string.IsNullOrEmpty(googleId) ||
                    string.IsNullOrEmpty(email))
                    throw new UnauthorizedException("login_incorreto");

                var expectedClientId = _appSettings.GoogleClientId;
                if (!string.IsNullOrEmpty(expectedClientId) &&
                    (!tokenInfo.TryGetValue("aud", out var aud) || aud != expectedClientId))
                    throw new UnauthorizedException("login_incorreto");
            }

            var usuario = _autenticacaoExternaRepositorio
                .Get(u => u.IdExterno.Equals(googleId) && u.Ativo)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.Grupo)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.UsuarioEndereco)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.Credenciamento)
                .Select(s => s.Usuario)
                .FirstOrDefault();

            if (usuario == null)
            {
                usuario = _repositorio.Get(u => u.Email.Equals(email) && u.Ativo)
                    .Include(t => t.Grupo)
                    .Include(t => t.UsuarioEndereco)
                    .Include(t => t.Credenciamentos)
                    .SingleOrDefault();

                if (usuario == null)
                    throw new UnauthorizedException("usuario_nao_encontrado");

                var provedorAutenticacao = await _provedorAutenticacaoNegocio
                    .FirstNoTrackingAsync(g =>
                        g.Protocolo == (int)IdentityProviderProtocol.Oauth2 &&
                        g.Provedor == (int)IdentityProvider.Google);

                var autenticacaoExterna = new AutenticacaoExterna
                {
                    IdExterno = googleId,
                    Ativo = true,
                    IdProvedorAutenticacao = provedorAutenticacao.IdProvedorAutenticacao,
                    IdUsuario = usuario.IdUsuario
                };
                _autenticacaoExternaRepositorio.Insert(autenticacaoExterna);
                _autenticacaoExternaRepositorio.SaveChanges();
            }

            var usuarioViewModel = Autenticacao(usuario, null, out Parceiro parceiro, false);
            return (usuarioViewModel, parceiro);
        }

        private static readonly object _appleJwksLock = new object();
        private static List<JsonWebKey> _appleJwksCache;
        private static DateTime _appleJwksExpiraEm = DateTime.MinValue;

        private async Task<List<JsonWebKey>> ObterAppleJwksAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && _appleJwksCache != null && _appleJwksExpiraEm > DateTime.UtcNow)
                return _appleJwksCache;

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
            var json = await client.GetStringAsync("https://appleid.apple.com/auth/keys");
            var keySet = new JsonWebKeySet(json);
            var keys = keySet.Keys.ToList();

            lock (_appleJwksLock)
            {
                _appleJwksCache = keys;
                _appleJwksExpiraEm = DateTime.UtcNow.AddHours(1);
            }
            return keys;
        }

        public async Task<(UsuarioViewModel usuario, Parceiro parceiro)> AutenticacaoAppleCredentialAsync(string identityToken, string emailFallback, string fullNameFallback)
        {
            if (string.IsNullOrEmpty(identityToken))
                throw new UnauthorizedException("login_incorreto");

            // Audience (Apple Service ID / bundle id) é obrigatório: sem isso, qualquer token Apple válido seria aceito
            var appleAudience = _appSettings?.AppleClientId;
            if (string.IsNullOrEmpty(appleAudience))
                throw new UnauthorizedException("provedor_nao_configurado");

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt;
            try { jwt = handler.ReadJwtToken(identityToken); }
            catch { throw new UnauthorizedException("login_incorreto"); }

            if (jwt.Issuer != "https://appleid.apple.com")
                throw new UnauthorizedException("login_incorreto");

            var jwks = await ObterAppleJwksAsync();
            var signingKey = jwks.FirstOrDefault(k => k.Kid == jwt.Header.Kid);
            if (signingKey == null)
            {
                // Apple pode ter rotacionado as chaves antes do TTL — tenta refresh forçado uma vez
                jwks = await ObterAppleJwksAsync(forceRefresh: true);
                signingKey = jwks.FirstOrDefault(k => k.Kid == jwt.Header.Kid);
                if (signingKey == null)
                    throw new UnauthorizedException("login_incorreto");
            }

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://appleid.apple.com",
                ValidateAudience = true,
                ValidAudience = appleAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
                IssuerSigningKey = signingKey,
                ValidateIssuerSigningKey = true
            };

            try { handler.ValidateToken(identityToken, validationParams, out _); }
            catch { throw new UnauthorizedException("login_incorreto"); }

            var appleSub = jwt.Subject;
            var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? emailFallback;
            if (string.IsNullOrEmpty(appleSub))
                throw new UnauthorizedException("login_incorreto");

            var usuario = _autenticacaoExternaRepositorio
                .Get(u => u.IdExterno.Equals(appleSub) && u.Ativo)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.Grupo)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.UsuarioEndereco)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.Credenciamento)
                .Select(s => s.Usuario)
                .FirstOrDefault();

            if (usuario == null)
            {
                if (string.IsNullOrEmpty(email))
                    throw new UnauthorizedException("usuario_nao_encontrado");

                usuario = _repositorio.Get(u => u.Email.Equals(email) && u.Ativo)
                    .Include(t => t.Grupo)
                    .Include(t => t.UsuarioEndereco)
                    .Include(t => t.Credenciamentos)
                    .SingleOrDefault();

                if (usuario == null)
                    throw new UnauthorizedException("usuario_nao_encontrado");

                var provedor = await _provedorAutenticacaoNegocio
                    .FirstNoTrackingAsync(g =>
                        g.Protocolo == (int)IdentityProviderProtocol.Oauth2 &&
                        g.Provedor == (int)IdentityProvider.Apple);

                if (provedor == null)
                    throw new UnauthorizedException("login_incorreto");

                var autenticacaoExterna = new AutenticacaoExterna
                {
                    IdExterno = appleSub,
                    Ativo = true,
                    IdProvedorAutenticacao = provedor.IdProvedorAutenticacao,
                    IdUsuario = usuario.IdUsuario
                };
                _autenticacaoExternaRepositorio.Insert(autenticacaoExterna);
                _autenticacaoExternaRepositorio.SaveChanges();
            }

            var usuarioViewModel = Autenticacao(usuario, null, out Parceiro parceiro, false);
            return (usuarioViewModel, parceiro);
        }

        private UsuarioViewModel Autenticacao(Usuario usuario, string senha, out Parceiro parceiro, bool verificarSenha = true)
        {
            parceiro = null;
            var limiteTentativas = 5;

            if (usuario == null)
                throw new UnauthorizedException("login_incorreto");

            if (usuario.Bloqueado == true)
            {
                if (usuario.TentativasIncorretas > limiteTentativas)
                {
                    throw new UnauthorizedException("login_limite_tentativa");
                }
                else
                {
                    throw new UnauthorizedException("login_bloqueado");
                }
            }
            else if (!usuario.Ativo)
            {
                throw new UnauthorizedException("usuario_inativo");
            }
            // check if password is correct
            else if ((verificarSenha && string.IsNullOrEmpty(senha)) || !Hash.CompareHashValue(senha, usuario.Email, usuario.Senha, usuario.SaltKey))
            {
                var parceiros = _parceiroNegocio.ObterParceiros(usuario.IdUsuario);
                parceiro = parceiros.FirstOrDefault(p => p.Celular == senha && p.Ativo);

                if (verificarSenha && parceiro == null)
                {
                    usuario.TentativasIncorretas = Convert.ToInt16(usuario.TentativasIncorretas.GetValueOrDefault() + 1);
                    throw new UnauthorizedException("login_incorreto");
                }
            }

            // autenticado
            if (usuario.TentativasIncorretas != 0)
            {
                usuario.TentativasIncorretas = 0;
                _repositorio.Update(usuario);
                _repositorio.SaveChanges();
            }

            return _mapper.Map<UsuarioViewModel>(usuario);
        }

        public UsuarioViewModel BuscarLoginOuEmail(string login)
        {
            return _mapper.Map<UsuarioViewModel>(_repositorio.GetByLoginOrEmail(login));
        }
        
        public bool AlterarSenha(string idUsuario, string senha, string senhaConfirma)
        {
            if (senha.Trim() != senhaConfirma.Trim()) return false;

            var guid = new Guid(idUsuario);

            return _repositorio.AlterarSenha(guid, senha);
        }
        
        public virtual UsuarioViewModel GetById(Guid idUsuario, params string[] entities)
        {
            var retorno = _repositorio.GetById(idUsuario, entities);
            return _mapper.Map<UsuarioViewModel>(retorno);
        }
        
        public List<UsuarioViewModel> ListaUsuarioDiretos(Guid idUsuario)
        {
            var usuarios = _repositorio.ListaUsuarioDiretos(idUsuario);
            return _mapper.Map<List<UsuarioViewModel>>(usuarios);
        }
        
        public List<UsuarioViewModel> ListaUsuariosPatrocinadores(Guid idUsuario, int? nivel)
        {
            var usuarios = _repositorio.ListaUsuariosPatrocinadores(idUsuario, nivel);
            return _mapper.Map<List<UsuarioViewModel>>(usuarios);
        }

        public LimitesGanhosViewModel BuscarLimitesGanhos(Guid idUsuarioLogado)
        {
            return _procedures.spc_LimitesGanhos(idUsuarioLogado);
        }
        
        public void ValidarContaUsuario(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            usuario.EmailConfirmado = true;
            Update(usuario);
        }
        
        public bool AtualizarDados(UsuarioViewModel dados)
        {
            return _repositorio.AtualizarDados(dados);
        }

        public List<Lancamento> ObterDistruibuicao(Guid idUsuario)
        {
            var lancamentos = _repositorio.ObterUltimosLancamentosPendentes(idUsuario);
            return lancamentos;
        }

        public object ObterDadosPessoais(Guid idUsuario)
        {
            var usuario = First(f => f.IdUsuario == idUsuario, "UsuarioPai");
            return new
            {
                usuario.Nome,
                usuario.Email,
                usuario.Login,
                usuario.Celular,
                usuario.Documento,
                LoginPatrocinador = usuario.UsuarioPai != null ? usuario.UsuarioPai.Login : string.Empty,
                usuario.RG,
                usuario.OrgaoEmissorRG,
                usuario.EstadoRG,
                usuario.PreCadastro,
                usuario.NomeSocial,
                usuario.Genero,
                usuario.LoginAlterado,
                usuario.DataNascimento
            };
        }
        
        public bool ObterDadosPessoaisPorCpfCnpj(string cpfCnpj)
        {
            return Exists(f => f.Documento == cpfCnpj, "UsuarioPai");
        }

        public object ObterFotoPerfil(Guid idUsuario)
        {
            var usuario = First(f => f.IdUsuario == idUsuario, "UsuarioPai");
            return new
            {
                usuario.UrlImg,
                usuario.IdUsuario
            };
        }

        public UsuarioViewModel AssinaturaEletronicaAleatoria(Guid idUsuario, string assinaturaEletronica)
        {
            var usuario = GetById(idUsuario);

            usuario.AssinaturaEletronica = Hash.Get_HASH_SHA512(assinaturaEletronica, usuario.Email, usuario.SaltKey);
            _repositorio.Update(_mapper.Map<Usuario>(usuario));
            _repositorio.SaveChanges();

            return usuario;
        }
        
        public bool VerificarSenha(Guid idUsuario, string senha)
        {
            var usuario = GetById(idUsuario);
            var result = Hash.CompareHashValue(senha, usuario.Email, usuario.Senha, usuario.SaltKey);
            return result;
        }

        public bool VerificarAssinaturaEletronica(Guid idUsuario, string assinatura)
        {
            var usuario = GetById(idUsuario);

            return Hash.CompareHashValue(assinatura, usuario.Email, usuario.AssinaturaEletronica, usuario.SaltKey);
        }

        public bool EditarDadosUsuario(UsuarioEditarViewModel viewModel)
        {
            try
            {
                var usuario = GetById(viewModel.IdUsuario);

                var usuarioEndereco = _usuarioEnderecoRepositorio.FirstNoTracking(x => x.IdUsuario == viewModel.IdUsuario);

                if (viewModel.Login != viewModel.Documento && viewModel.Login != usuario.Login && _repositorio.Any(x => x.Login == viewModel.Login))
                    throw new NotFoundException("login_em_uso");


                if (viewModel.Login != usuario.Login && !usuario.LoginAlterado)
                {
                    usuario.LoginAlterado = true;
                    usuario.Login = viewModel.Login;
                }
                else if (viewModel.Login != usuario.Login && usuario.LoginAlterado)
                {
                    throw new PadraoException("login_alterado");
                }

                if (usuarioEndereco != null || usuario.DataNascimento != null || usuario.Login != usuario.Documento)
                {
                    usuario.PreCadastro = false;
                }
                usuario.Nome = viewModel.Nome;
                usuario.Celular = viewModel.Celular.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");

                var usuarioDiferenteMesmoTelefone = FirstNoTracking(u => u.Celular == usuario.Celular && u.IdUsuario != usuario.IdUsuario);

                //if (usuarioDiferenteMesmoTelefone != null)
                //    throw new PadraoException("telefone_em_uso");

                usuario.Documento = UtilBase.FiltrarDigitos(viewModel.Documento);

                var usuarioDiferenteMesmoDocumento = FirstNoTracking(u => u.Documento == usuario.Documento && u.IdUsuario != usuario.IdUsuario);
                if (usuarioDiferenteMesmoDocumento != null)
                {
                    throw new PadraoException("cpf_cnpj_em_uso");
                }

                usuario.Nome = usuario.Nome.ToUpper();
                usuario.Login = usuario.Login.ToLower();
                usuario.RG = viewModel.RG;
                usuario.OrgaoEmissorRG = viewModel.OrgaoEmissorRG;
                usuario.EstadoRG = viewModel.EstadoRG;
                usuario.DataNascimento = viewModel.DataNascimento;
                usuario.Genero = viewModel.Genero;
                usuario.NomeSocial = (viewModel.NomeSocial ?? string.Empty).ToUpper();

                if (!string.IsNullOrEmpty(viewModel.NovaSenha))
                {
                    usuario.Senha = Hash.Get_HASH_SHA512(viewModel.NovaSenha, usuario.Email, usuario.SaltKey);
                }

                _repositorio.Update(_mapper.Map<Usuario>(usuario));
                _repositorio.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("EditarDadisUsuario(UsuarioEditarViewModel viewModel)", ex, "UsuarioNegocio");
                throw;
            }
        }
        
        public bool UpdateImage(Guid idUsuario, string image)
        {
            try
            {
                var usuario = GetById(idUsuario);
                usuario.UrlImg = image;

                _repositorio.Update(_mapper.Map<Usuario>(usuario));
                _repositorio.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Updateimage(Guid idUsuario, string image)", ex, "UsuarioNegocio");
                return false;
            }
        }
        
        public bool UpdateAssinaturaEletronica(Guid idUsuario, string assinaturaEletronica)
        {
            try
            {
                var usuario = GetById(idUsuario);
                usuario.AssinaturaEletronica = Hash.Get_HASH_SHA512(assinaturaEletronica, usuario.Email, usuario.SaltKey); ;

                _repositorio.Update(_mapper.Map<Usuario>(usuario));
                _repositorio.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Updateimage(Guid idUsuario, string image)", ex, "UsuarioNegocio");
                return false;
            }
        }

        public object FiltrarUsuarios(FiltroViewModel.FiltroUsuario filtroUsuario)
        {
            var query = _repositorio.GetAllNoTracking()
         .Where(w =>
             (!string.IsNullOrEmpty(filtroUsuario.Login) ? w.Login.ToLower().StartsWith(filtroUsuario.Login.ToLower()) : true) &&
             (!string.IsNullOrEmpty(filtroUsuario.Celular) ? w.Celular.Contains(filtroUsuario.Celular) : true) &&
             (!string.IsNullOrEmpty(filtroUsuario.Nome) ? w.Nome.ToLower().StartsWith(filtroUsuario.Nome) : true) &&
             (filtroUsuario.IdProduto.HasValue ? w.UsuarioProduto.Any(a => a.IdUsuario == w.IdUsuario && a.Ativo && a.IdProduto == filtroUsuario.IdProduto.Value) : true) &&
             (!string.IsNullOrEmpty(filtroUsuario.Email) ? w.Email.Contains(filtroUsuario.Email) : true) &&
             !w.Master &&
             (filtroUsuario.Ativo.HasValue ? w.Ativo == filtroUsuario.Ativo : true) &&
             (filtroUsuario.IdGraduacao.HasValue ? w.IdGraduacao == filtroUsuario.IdGraduacao : true) &&
             (!string.IsNullOrEmpty(filtroUsuario.Patrocinador) ? w.UsuarioPai.Login.ToLower().Contains(filtroUsuario.Patrocinador.ToLower()) : true)
         )
         .Include("UsuarioPai")
         .Include("Graduacao")
         .Include("UsuarioProduto")
         .Include("UsuarioProduto.Produto")
         .Include("Credenciamentos")
         .Include("Grupo");

            switch (filtroUsuario.OrdenacaoUsuarios)
            {
                case EnumOrdenacaoUsuarios.Email:
                    query = filtroUsuario.OrderDesc ? query.OrderByDescending(o => o.Email) : query.OrderBy(o => o.Email);
                    break;
                case EnumOrdenacaoUsuarios.Login:
                    query = filtroUsuario.OrderDesc ? query.OrderByDescending(o => o.Login) : query.OrderBy(o => o.Login);
                    break;
                case EnumOrdenacaoUsuarios.Nome:
                    query = filtroUsuario.OrderDesc ? query.OrderByDescending(o => o.Nome) : query.OrderBy(o => o.Nome);
                    break;
                case EnumOrdenacaoUsuarios.Patrocinador:
                    query = filtroUsuario.OrderDesc ? query.OrderByDescending(o => o.UsuarioPai.Login) : query.OrderBy(o => o.UsuarioPai.Login);
                    break;
                case EnumOrdenacaoUsuarios.DataCadastro:
                    query = filtroUsuario.OrderDesc ? query.OrderByDescending(o => o.DataCadastro) : query.OrderBy(o => o.DataCadastro);
                    break;
            }

            var total = query.Count();

            var listaUsuarios = query
        .Skip((filtroUsuario.Page - 1) * filtroUsuario.Quantidade)
        .Take(filtroUsuario.Quantidade)
        .AsEnumerable();

            var totalPages = (int)Math.Ceiling((double)total / filtroUsuario.Quantidade);

            var usuariosFiltrados = listaUsuarios
            .Select(s => new
            {
                s.IdUsuario,
                s.Nome,
                s.Login,
                s.Email,
                s.Bloqueado,
                s.Ativo,
                s.DataCadastro,
                s.EmailConfirmado,
                s.Celular,
                credenciamento = s.IdGrupo == 10, //Comerciante
                nomeProduto = s.UsuarioProduto != null && s.UsuarioProduto.Any(a => a.Ativo) ? s.UsuarioProduto.FirstOrDefault(p => p.Ativo).Produto.Nome : string.Empty,
                Graduacao = s.Graduacao.Nome,
                LoginPatrocinador = s.UsuarioPai != null ? s.UsuarioPai.Login : "",
                Hibrido = s.Empreendedor && s.Grupo != null && s.Grupo.Descricao == "Credenciado"
            }).ToList();

            return new { totalPages, filtroUsuario.Quantidade, filtroUsuario.Page, usuariosFiltrados, quantidadeTotal = listaUsuarios.Count() };
        }

        public bool AdminAtualizarDadosUsuario(AdminDadosUsuarioViewModel dados)
        {
            return _repositorio.AdminAtualizarDadosUsuario(dados);
        }

        public bool VerificaLoginDisponivel(UsuarioViewModel usuario, string login)
        {
            return _repositorio.VerificaLoginDisponivel(_mapper.Map<Usuario>(usuario), login);
        }

        public bool VerificaPatrocinador(string loginPatrocinador)
        {
            return _repositorio.VerificaPatrocinador(loginPatrocinador);
        }

        public bool BloquearUsuarioAdmin(Guid idUsuario)
        {
            return _repositorio.BloquearUsuarioAdmin(idUsuario);
        }

        public List<UsuarioViewModel> ObterTodosAdministrativos(UsuarioViewModel model)
        {
            return _mapper.Map<List<UsuarioViewModel>>(_repositorio.ObterTodosAdministrativos(model));
        }

        public UsuarioViewModel GetByLoginOrEmail(string login)
        {
            return _mapper.Map<UsuarioViewModel>(_repositorio.GetByLoginOrEmail(login));
        }

        public PontuacaoPorValorViewModel GetPontosFromCache(Guid idUsuario)
        {
            var pontos = (PontuacaoPorValorViewModel)_cache.GetItem(idUsuario.ToString());
            if (pontos != null)
            {
                return pontos;
            }

            pontos = _procedures.spc_GetPontuacaoUsuarioPorValor(idUsuario);
            _cache.SetItem(idUsuario.ToString(), pontos, DateTime.UtcNow.AddDays(1).Date);

            return pontos;
        }

        public decimal? GetPontosPremiacaoFromCache(Guid idUsuario, int porcentagem, int totalPontos, int idGraduacao)
        {
            var pontos = (decimal?)_cache.GetItem(idUsuario.ToString() + "_Premiacao");
            if (pontos != null) return pontos;

            _cache.SetItem(idUsuario.ToString() + "_Premiacao", _procedures.spc_ObterPontuacaoUsuarioPremiacao(idUsuario, porcentagem, totalPontos, idGraduacao));
            pontos = (decimal)_cache.GetItem(idUsuario.ToString() + "_Premiacao");

            return pontos;
        }

        public IList<RankUsuarioViewModel> GetRankFromCache(Guid idUsuario)
        {
            var rank = (List<RankUsuarioViewModel>)_cache.GetItem(idUsuario.ToString() + "_Rank");
            if (rank != null) return rank;

            _cache.SetItem(idUsuario.ToString() + "_Rank", _procedures.spc_ObterRankUsuario(idUsuario));
            rank = (List<RankUsuarioViewModel>)_cache.GetItem(idUsuario.ToString() + "_Rank");

            return rank;
        }

        public List<UsuarioViewModel> ObterUltimosDiretos(Guid idUsuario)
        {
            var usuarios = _repositorio.ObterUltimosDiretos(idUsuario);
            return _mapper.Map<List<UsuarioViewModel>>(usuarios);
        }

        public IList<RankUsuarioViewModel> ObterRankFiltrado(Guid idUsuario, string login, string ordenacao)
        {
            var rank = _procedures.spc_ObterRankUsuarioFiltrado(idUsuario, login, ordenacao);
            return rank;
        }

        public UsuarioViewModel CadastrarUsuarioComPlanoIntegracao(UsuarioViewModel usuario, int idPlano)
        {
            var grupoUsuario = _grupoNegocio.GetByName("Usuario");
            var graduacao = _graduacaoNegocio.ObterMenorNivel();

            var usuarioModel = _mapper.Map<Usuario>(usuario);

            //Verifica login Pai
            var usuarioPai = _repositorio.FirstNoTracking(up => up.Login == "triumph");
            //if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
            //    throw new NotFoundException("patrocinador_nao_encontrado");

            //if (usuarioPai.DataReferencia.HasValue == false)
            //{
            //    throw new PadraoException("patrocinador_inativo");
            //}

            //Verifica login em uso
            var usuarioLogin = _repositorio.FirstNoTracking(u => u.Login == usuario.Login);

            if (usuarioLogin != null)
                return _mapper.Map<UsuarioViewModel>(usuarioLogin);

            var salt = Hash.Get_SALT();
            usuarioModel.SaltKey = salt;
            usuarioModel.Ativo = true;
            usuarioModel.Cultura = "pt-BR";
            usuarioModel.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
            usuarioModel.EmailConfirmado = false;
            usuarioModel.Bloqueado = false;
            usuarioModel.TentativasIncorretas = 0;
            usuarioModel.IdGraduacao = graduacao.IdGraduacao;
            usuarioModel.PosicaoBinario = Convert.ToInt16(usuarioPai.PosicaoBinario.GetValueOrDefault() == 0 ? 1 : 0);
            //usuarioModel.PosicaoBinarioPai = usuarioPai.PosicaoBinario.GetValueOrDefault();
            usuarioModel.IdGrupo = grupoUsuario.IdGrupo;
            usuarioModel.Senha = Hash.Get_HASH_SHA512(usuario.Senha, usuario.Email, salt);
            usuarioModel.UrlImg = "https://bigcash.blob.core.windows.net/imagens-perfil/10b57d04-d0ef-43ca-bb2f-fbc8aa5f1b812020-11-10-17-19-19.jpeg";
            usuarioModel.IdUsuarioPai = usuarioPai.IdUsuario;
            usuarioModel.DataReferencia = DateTime.UtcNow.HorarioBrasilia();
            //usuarioModel.UsuarioAdministrativo = false;

            switch (idPlano)
            {
                case 3:
                    idPlano = 1;
                    break;
                case 5:
                    idPlano = 2;
                    break;
                case 9:
                    idPlano = 6;
                    break;
                default:
                    idPlano = 6;
                    break;
            }

            return _mapper.Map<UsuarioViewModel>(_repositorio.CadastrarUsuarioComPlanoIntegracao(usuarioModel, idPlano));
        }

        public BarraDeStatusViewModel GetBarraStatusFromCache(Guid idUsuario)
        {
            var barraDeStatus = (BarraDeStatusViewModel)_cache.GetItem(idUsuario.ToString() + "_BarraStatus");
            //if (barraDeStatus != null) return barraDeStatus;

            barraDeStatus = _procedures.spc_obterBarraDeStatus(idUsuario);

            _cache.SetItem(idUsuario.ToString() + "_BarraStatus", barraDeStatus);
            barraDeStatus = (BarraDeStatusViewModel)_cache.GetItem(idUsuario.ToString() + "_BarraStatus");

            return barraDeStatus;
        }

        public async Task<RefreshTokenResponseViewModel> GenerateTokenAndRefreshToken(UsuarioViewModel usuario, bool comerciante)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, usuario.Grupo.Descricao),
                    new Claim(ClaimTypes.PrimarySid, usuario.Grupo.IdGrupo.ToString()),
                    new Claim(ClaimTypes.PrimaryGroupSid, usuario.IdGraduacao.HasValue ? usuario.IdGraduacao.ToString() : "0"),
                    new Claim("Cultura", usuario.Cultura),
                    new Claim("Comerciante", comerciante.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshTokenViewModel
            {
                JwtId = token.Id,
                IdUsuario = usuario.IdUsuario,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                Used = false,
                Invalidated = false
            };

            refreshToken = await _refreshTokenNegocio.AddNewToken(refreshToken);

            return new RefreshTokenResponseViewModel
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<RefreshTokenResponseViewModel> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
                return new RefreshTokenResponseViewModel { Error = "Token inválido.", ErrorType = EnumErrorTypeRefreshToken.Invalido };

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
                return new RefreshTokenResponseViewModel { Error = "Este token ainda não expirou.", ErrorType = EnumErrorTypeRefreshToken.TokenNaoExpirado };

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _refreshTokenNegocio.FirstNoTracking(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
                return new RefreshTokenResponseViewModel { Error = "Esse refresh token não existe.", ErrorType = EnumErrorTypeRefreshToken.TokenNaoExistente };

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new RefreshTokenResponseViewModel { Error = "Esse refresh token expirou.", ErrorType = EnumErrorTypeRefreshToken.TokenExpirado };

            if (storedRefreshToken.Invalidated)
                return new RefreshTokenResponseViewModel { Error = "Esse refresh token foi inválidado.", ErrorType = EnumErrorTypeRefreshToken.TokenInvalidado };

            if (storedRefreshToken.Used)
                return new RefreshTokenResponseViewModel { Error = "Esse refresh token já foi usado.", ErrorType = EnumErrorTypeRefreshToken.TokenJaUtilizado };

            if (storedRefreshToken.JwtId != jti)
                return new RefreshTokenResponseViewModel { Error = "Esse refresh token não bate com o JWT.", ErrorType = EnumErrorTypeRefreshToken.TokenNaoConfere };

            storedRefreshToken.Used = true;
            _refreshTokenNegocio.Update(storedRefreshToken);
            var user = GetById(Guid.Parse(validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value), "Grupo");

            return await GenerateTokenAndRefreshToken(user, user.Grupo.Descricao == "Comerciante");
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // o metodo para pegar o principal do token estava dando erro pelo lifetime, por isso altero a validação para false e
                // após a validação aciono novamente
                _tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                _tokenValidationParameters.ValidateLifetime = true;
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}