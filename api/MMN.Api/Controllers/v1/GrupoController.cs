using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoNegocio _grupoNegocio;
        private readonly IGrupoMenuNegocio _grupoMenuNegocio;
        private readonly IMenuNegocio _menuNegocio;
        private readonly ICache _cache;
        private readonly IUsuarioNegocio _negocioUsuario;

        public GrupoController(IGrupoNegocio negocio, IUsuarioNegocio negociousuario, IMenuNegocio menuNegocio, IGrupoMenuNegocio grupoMenuNegocio, ICache cache)
        {
            _grupoNegocio = negocio;
            _negocioUsuario = negociousuario;
            _menuNegocio = menuNegocio;
            _grupoMenuNegocio = grupoMenuNegocio;
            _cache = cache;
        }

        [HttpGet, Route("listarGrupos")]
        public IActionResult ListarGrupos()
        {
            return Ok(_grupoNegocio.GetAll().ToList());
        }

        [HttpPut, Route("ativaDesativaGrupo")]
        public IActionResult AtivaDesativaGrupo(GrupoViewModel viewModel)
        {
            var grupo = _grupoNegocio.FirstNoTracking(f => f.IdGrupo == viewModel.IdGrupo);
            grupo.Ativo = !grupo.Ativo;
            _grupoNegocio.Update(grupo);

            return Ok(new { status = grupo.Ativo, message = "Status atualizado com sucesso!" });
        }

        [HttpPost, Route("cadastrarGrupo")]
        public IActionResult CadastrarGrupo(GrupoViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Descricao))
            {
                viewModel.Ativo = true;
                _grupoNegocio.Insert(viewModel);

                return Ok(new { message = "Grupo cadastrado com sucesso!" });
            }

            return Ok(new { message = "Descrição não pode ser vazia!" });
        }

        [HttpGet, Route("buscarMenusPorGrupo/{idGrupo}")]
        public IActionResult BuscarMenusPorGrupo(int idGrupo)
        {
            var menus = _menuNegocio.Get(w => w.Ativo && !w.RotaPublica && w.IdMenuPai == null, "SubMenus").OrderBy(o => o.Texto).ToList();
            var grupoMenu = _grupoMenuNegocio.Get(w => w.IdGrupo == idGrupo).ToList();

            var teste = _grupoMenuNegocio.SelecionarMenus(menus, grupoMenu);
            return Ok(teste);
        }

        [HttpPost, Route("ativarDesativarPermissao")]
        public IActionResult AtivarDesativarPermissao(List<GrupoMenuViewModel> viewModel)
        {
            foreach (var grupoMenu in viewModel)
            {
                var grupoMenuEncontrado = _grupoMenuNegocio.FirstNoTracking(w => w.IdGrupo == grupoMenu.IdGrupo && w.IdMenu == grupoMenu.IdMenu);

                if (grupoMenuEncontrado == null)
                {
                    grupoMenu.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
                    grupoMenu.IdUsuarioAcao = 1;
                    grupoMenu.Acesso = true;
                    grupoMenu.SomenteLeitura = false;

                    _grupoMenuNegocio.Insert(grupoMenu);
                }
                else
                {
                    var idMenusFilhos = _menuNegocio.Get(w => w.IdMenuPai == grupoMenuEncontrado.IdMenu).Select(s => s.IdMenu).ToArray();
                    if (idMenusFilhos.Count() > 0)
                    {
                        var gruposMenusFilhos = _grupoMenuNegocio.GetNoTracking(w => idMenusFilhos.Contains(w.IdMenu) && w.IdGrupo == grupoMenuEncontrado.IdGrupo).ToList();
                        _grupoMenuNegocio.DeleteRange(gruposMenusFilhos);
                    }

                    _grupoMenuNegocio.Delete(grupoMenuEncontrado.IdGrupoMenu.Value);
                }
                _cache.RemoveItem(CacheKeys.GroupMenu + "_" + grupoMenu.IdGrupo.ToString());

            }

            return Ok(new { message = "Permissão alterada com sucesso!" });
        }
    }
}