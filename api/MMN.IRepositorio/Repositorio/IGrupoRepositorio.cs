using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;

namespace MMN.IRepositorio.Repositorio
{
    public interface IGrupoRepositorio : IBaseRepositorio<Grupo>
    {
        Grupo GetByName(string nome);
    }
}
