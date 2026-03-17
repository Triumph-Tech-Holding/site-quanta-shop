using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Negocio.Negocio
{
    public class GrupoMenuNegocio : BaseNegocio<GrupoMenuViewModel, GrupoMenu>, IGrupoMenuNegocio
    {
        private readonly IGrupoMenuRepositorio _repositorio;
        private readonly IMapper _mapper;
        public GrupoMenuNegocio(IGrupoMenuRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public List<object> SelecionarMenus(List<MenuViewModel> menus, List<GrupoMenuViewModel> grupoMenus)
        {
            var listMenus = new List<object>();

            foreach (var menu in menus)
            {
                if (grupoMenus.Any(a => a.IdMenu == menu.IdMenu))
                {
                    listMenus.Add(new
                    {
                        menu.IdMenu,
                        menu.Texto,
                        VendoMenu = true,
                        SubMenus = VerificaFilhos(menu.SubMenus, grupoMenus)
                    });
                }
                else
                {
                    listMenus.Add(new
                    {
                        menu.IdMenu,
                        menu.Texto,
                        VendoMenu = false,
                        SubMenus = menu.SubMenus.Select(s => new
                        {
                            s.IdMenu,
                            s.Texto,
                            VendoMenu = false,
                            s.IdMenuPai
                        }).ToList()
                    });
                }
            }

            return listMenus;
        }

        private List<object> VerificaFilhos(List<Menu> subMenus, List<GrupoMenuViewModel> grupoMenus)
        {
            var listSubMenus = new List<object>();

            foreach (var subMenu in subMenus)
            {
                if (grupoMenus.Any(a => a.IdMenu == subMenu.IdMenu))
                {
                    listSubMenus.Add(new
                    {
                        subMenu.IdMenu,
                        subMenu.Texto,
                        VendoMenu = true,
                        subMenu.IdMenuPai
                    });
                }
                else
                {
                    listSubMenus.Add(new
                    {
                        subMenu.IdMenu,
                        subMenu.Texto,
                        VendoMenu = false,
                        subMenu.IdMenuPai
                    });
                }
            }

            return listSubMenus;
        }
    }
}
