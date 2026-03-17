using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class MenuRepositorio : BaseRepositorio<Menu>, IMenuRepositorio
    {
        public MenuRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Menu> ObterMenuPorGrupo(int idGrupo)
        {
            return _ctx.Menu.Where(m => m.GrupoMenu.Any(g => g.IdGrupo == idGrupo) && m.Ativo == true).OrderBy(m => m.Posicao).Include("SubMenus").Where(s => s.Ativo).OrderBy(s => s.Posicao).ToList();
        }
    }
}
