using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.IRepositorio.Repositorio
{
    public interface IGraduacaoRepositorio : IBaseRepositorio<Graduacao>
    {
        Graduacao ObterPorNivel(int nivel);
        Graduacao ObterMenorNivel();
    }
}
