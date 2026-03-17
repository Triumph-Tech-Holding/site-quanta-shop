using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;

namespace MMN.INegocio.Negocio
{
    public interface IGraduacaoNegocio : IBaseNegocio<GraduacaoViewModel, Graduacao>
    {
        GraduacaoViewModel ObterPorNivel(int nivel);
        GraduacaoViewModel ObterMenorNivel();
        List<GraduacaoViewModel> GetFromCache();
    }
}