using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.IRepositorio.Repositorio
{
    public interface ICategoriaRepositorio : IBaseRepositorio<Categoria>
    {
        List<Categoria> BuscarAtivos(string chavePai);
        List<Categoria> BuscarCategorias();

        List<CategoriaViewModel> ObterCategorias(FiltroHomeCategoriaViewModel filtro);

        List<Credenciamento> GetCredenciamentosParaCategorias(int id);

        List<Credenciamento> GetStatusCredenciamentos(List<long?> idCredenciamentos);

        List<CategoriaAnunciante> ObterMapeamentos();
    } 
}
