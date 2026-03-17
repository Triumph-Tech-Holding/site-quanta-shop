using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.INegocio.Negocio
{
    public interface ITutorialNegocio : IBaseNegocio<TutorialViewModel, Tutorial>
    {
        void CriarTutorial(TutorialViewModel viewModel);
        void EditarTutorial(TutorialViewModel viewModel);
    }
}
