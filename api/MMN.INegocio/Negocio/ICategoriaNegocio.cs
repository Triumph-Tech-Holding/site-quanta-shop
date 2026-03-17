using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.INegocio.Negocio
{
    public interface ICategoriaNegocio : IBaseNegocio<CategoriaViewModel, Categoria>
    {
        List<CategoriaViewModel> BuscarAtivos(string chavePai);
        List<CategoriaViewModel> GetCategorias(FiltroHomeCategoriaViewModel filtro);
        List<Categoria> BuscarCategorias();
        List<CategoriaViewModel> BuscarCategoriasAleatorias();
        public Categoria CriarCategoria(CriarCategoriaViewModel categoria);
        public Categoria AtualizarCategoria(CriarCategoriaViewModel categoria);
        public void DeletarCategoria(int idCategoria);
        public List<CategoriaAnunciante> ObterMapeamentos();
        public CategoriaAnuncianteViewModel Map(CategoriaAnuncianteMapViewModel map);
        public bool RemoveMap(int idMap);
    }
}
