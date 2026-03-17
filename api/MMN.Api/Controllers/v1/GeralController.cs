using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Models;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Model;
using MMN.Util.Translation;
using MMN.Util.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeralController : LoggedControllerBase
    {
        private readonly IMenuNegocio _negocioMenu;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly IUsuarioNegocio _negocioUsuario;
        private readonly ITipoNegocio _negocioTipo;
        private readonly IStatusNegocio _negocioStatus;
        private readonly ICidadeNegocio _negocioCidade;
        private readonly IEstadoNegocio _negocioEstado;
        private readonly AppSettings _appSettings;
        private readonly ILocation _location;
        private readonly ICache _cache;
        private readonly IGraduacaoNegocio _graduacaoNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly IFaqNegocio _faqNegocio;
        private readonly IUsuarioPremiacaoNegocio _usuarioPremiacaoNegocio;
        private readonly IConfiguracaoNegocio _configNegocio;

        public GeralController(
            IMenuNegocio negocioMenu,
            IUsuarioNegocio negocioUsuario,
            IOptions<AppSettings> appSettings,
            ILocation location,
            ITipoNegocio negocioTipo,
            ICidadeNegocio negocioCidade,
            IEstadoNegocio negocioEstado,
            ICache cache,
            IStatusNegocio negocioStatus,
            IGraduacaoNegocio graduacaoNegocio,
            ICredenciamentoNegocio credenciamentoNegocio,
            IFaqNegocio faqNegocio,
            ICategoriaNegocio categoriaNegocio,
            IUsuarioPremiacaoNegocio usuarioPremiacaoNegocio,
            IConfiguracaoNegocio configNegocio)
        {
            _negocioMenu = negocioMenu;
            _negocioUsuario = negocioUsuario;
            _appSettings = appSettings.Value;
            _location = location;
            _negocioTipo = negocioTipo;
            _negocioCidade = negocioCidade;
            _negocioEstado = negocioEstado;
            _cache = cache;
            _negocioStatus = negocioStatus;
            _graduacaoNegocio = graduacaoNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _faqNegocio = faqNegocio;
            _usuarioPremiacaoNegocio = usuarioPremiacaoNegocio;
            _categoriaNegocio = categoriaNegocio;
            _configNegocio = configNegocio;
        }

        [HttpGet]
        [Route("obterMenu/{perfil}")]
        public IActionResult ObterMenu(char perfil)
        {
            var usuario = _negocioUsuario.GetById(IdUsuarioLogado, "Grupo");

            if (usuario.Grupo.Descricao.Equals("Comerciante"))
            {
                //var menus = (IList<MenuViewModel>)_cache.GetItem(CacheKeys.GroupMenu + "_" + (perfil.Equals('C') ? usuario.Grupo.IdGrupo.ToString() : "2"));
                //if (menus == null)
                //{
                var menus = _negocioMenu.ObterMenuPorGrupo(perfil.Equals('C') ? usuario.Grupo.IdGrupo : 2);
                //_cache.SetItem(CacheKeys.GroupMenu + "_" + (perfil.Equals('C') ? usuario.Grupo.IdGrupo.ToString() : "2"), menus);
                //}

                return Ok(menus.Where(m => m.MenuPrincipal).OrderBy(y => y.Posicao).Select(m => new
                {
                    m.IdMenu,
                    m.Posicao,
                    m.Texto,
                    m.Url,
                    m.MenuPrincipal,
                    m.ChaveTraducao,
                    m.IdMenuPai,
                    SubMenus = m.SubMenus.OrderBy(y => y.Posicao).Select(s => new
                    {
                        s.IdMenu,
                        s.Posicao,
                        s.Texto,
                        s.Url,
                        s.MenuPrincipal,
                        s.ChaveTraducao,
                        s.IdMenuPai,
                    })
                }));
            }

            var menu = (IList<MenuViewModel>)_cache.GetItem(CacheKeys.GroupMenu + "_" + IdGrupoLogado.ToString());
            if (menu == null)
            {
                menu = _negocioMenu.ObterMenuPorGrupo(IdGrupoLogado);
                _cache.SetItem(CacheKeys.GroupMenu + "_" + IdGrupoLogado.ToString(), menu);
            }

            var menusFiltrados = menu.Where(m => m.MenuPrincipal).OrderBy(y => y.Posicao).Select(m => new
            {
                m.IdMenu,
                m.Posicao,
                m.Texto,
                m.Url,
                m.MenuPrincipal,
                m.ChaveTraducao,
                m.IdMenuPai,
                SubMenus = m.SubMenus.OrderBy(y => y.Posicao).Select(s => new
                {
                    s.IdMenu,
                    s.Posicao,
                    s.Texto,
                    s.Url,
                    s.MenuPrincipal,
                    s.ChaveTraducao,
                    s.IdMenuPai,
                    SubMenus = s.SubMenus != null ? s.SubMenus.OrderBy(y => y.Posicao).Select(x => new
                    {
                        x.IdMenu,
                        x.Posicao,
                        x.Texto,
                        x.Url,
                        x.MenuPrincipal,
                        x.ChaveTraducao,
                        x.IdMenuPai,
                    }) : null
                })
            });

            return Ok(menusFiltrados);
        }

        [HttpGet]
        [Route("obterDadosApp")]
        public async Task<IActionResult> ObterDadosTelaApp()
        {
            var imagensSlider = AzureStorage.GetImages(_appSettings.StorageAccountConnectionString, "screenshots-app-slider").Result;
            var imagensCarrossel = AzureStorage.GetImages(_appSettings.StorageAccountConnectionString, "screenshots-app-carrossel").Result;

            var urlAndroid = _configNegocio.FirstNoTracking(c => c.Chave == "URL_APP_ANDROID");
            var urlApple = _configNegocio.FirstNoTracking(c => c.Chave == "URL_APP_IOS");

            var comentariosString = _configNegocio.FirstNoTracking(c => c.Chave == "COMENTARIOS_APP");
            //var comentariosParsed = JsonConvert.DeserializeObject<List<Comentario>>(comentariosString.Valor);

            return Ok(new
            {
                imagensSlider,
                imagensCarrossel,
                urlApple = urlApple.Valor,
                urlAndroid = urlAndroid.Valor,
                //comentarios = comentariosParsed
            });
        }


        [HttpGet]
        [Route("tipo/{chave}")]
        public IActionResult Tipo(string chave)
        {
            var tipo = _negocioTipo.ObterTipoPorChave(chave);
            return Ok(tipo.Select(t => new
            {
                t.IdTipo,
                t.Descricao,
                t.Chave
            }));
        }

        [HttpGet]
        [Route("buscaCidade/{idEstado}")]
        public IActionResult BuscaCidadePorEstado(int idEstado)
        {
            var lstCidadeEstado = _negocioCidade.BuscarCidadeEstado(idEstado);

            return Ok(lstCidadeEstado.Select(c => new
            {
                c.IdCidade,
                c.Nome
            }));
        }

        [HttpGet]
        [Route("buscaEstado")]
        public IActionResult BuscaEstado()
        {
            var lstEstado = _negocioEstado.BuscarEstado();

            return Ok(lstEstado.Select(c => new
            {
                c.IdEstado,
                c.Nome,
                c.Uf
            }));
        }

        [HttpGet]
        [Route("status")]
        public IActionResult Status()
        {
            var tipo = _negocioStatus.GetFromCache();
            return Ok(tipo.Select(t => new
            {
                t.IdStatus,
                t.Nome
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("obterGraduacoes")]
        public IActionResult ObterGraduacoes()
        {
            return Ok(_graduacaoNegocio.Get(g => g.IdGraduacao != 1));
        }

        [HttpGet]
        [Authorize]
        [Route("obterGraduacoesPontoUsuario")]
        public IActionResult ObterGraduacoesPontoUsuario()
        {
            var graduacoes = _graduacaoNegocio.GetFromCache().Where(w => w.PercentualPremiacao.HasValue).Select(s => new
            {
                s.Nome,
                s.PercentualPremiacao,
                s.Pontos,
                MeusPontos = s.IdGraduacao == IdGraduacaoLogado ?
                    _negocioUsuario.GetPontosPremiacaoFromCache(IdUsuarioLogado, s.PercentualPremiacao.Value, s.Pontos.Value, IdGraduacaoLogado).ToString().Replace(",", ".") : "---",
                s.Premio,
                s.IdGraduacao,
                Ganhou = s.IdGraduacao == IdGraduacaoLogado ?
                    _negocioUsuario.GetPontosPremiacaoFromCache(IdUsuarioLogado, s.PercentualPremiacao.Value, s.Pontos.Value, IdGraduacaoLogado) >= s.Pontos : false
            }).ToList();

            return Ok(new { graduacoes, IdGraduacaoLogado });
        }

        [HttpGet]
        [Authorize]
        [Route("obterGraduacoesDetalhes")]
        public IActionResult ObterGraduacoesDetalhes()
        {
            var graduacoes = _graduacaoNegocio.Get(g => g.IdGraduacao != 1, "GraduacaoRequisitos");

            return Ok(new
            {
                graduacoes,
                pontosUsuario = _negocioUsuario.GetPontosFromCache(IdUsuarioLogado).PontosRedeElegivel,
                IdGraduacaoLogado
            });
        }

        [HttpGet, AllowAnonymous, Route("ObterCategorias")]
        public IActionResult obterCategorias()
        {
            return Ok(_categoriaNegocio.Get(c => c.Ativo).OrderBy(c => c.Nome));
        }

        [HttpPost]
        [Route("InsertUsersCsv")]
        public async Task<IActionResult> InsertUsersSvg(InsertUsersViewModel viewModel)
        {
            var file = Convert.FromBase64String(viewModel.Base64);
            string[] csv = Encoding.UTF8.GetString(file).Split("\n");
            var listaUsuarios = new List<UsuarioViewModel>();
            UsuarioViewModel novoUsuario;

            for (int i = 0; i < csv.Length; i++)
            {
                if (!string.IsNullOrEmpty(csv[i]))
                {
                    string[] linha = csv[i].Replace("\"", "").Split(",");

                    novoUsuario = new UsuarioViewModel
                    {
                        Login = linha[1],
                        Email = linha[2],
                        Celular = linha[3],
                        Nome = linha[4] + " " + linha[5],
                        Senha = "trocar@123"
                    };
                    novoUsuario = _negocioUsuario.CadastrarUsuarioComPlanoIntegracao(novoUsuario, int.Parse(linha[9]));


                    if (novoUsuario != null)
                    {
                        novoUsuario.LoginPatrocinador = linha[8];
                        listaUsuarios.Add(novoUsuario);
                    }
                }
            }

            listaUsuarios.Select(s =>
            {
                var usuarioPai = _negocioUsuario.FirstNoTracking(u => u.Login == s.LoginPatrocinador && u.UsuarioPai.Login != s.LoginPatrocinador);
                if (usuarioPai != null)
                    s.IdUsuarioPai = usuarioPai.IdUsuario;
                return s;
            }).ToList();

            _negocioUsuario.UpdateRange(listaUsuarios);

            return Ok();
        }
    }
}