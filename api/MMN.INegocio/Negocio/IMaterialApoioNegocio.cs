using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.INegocio.Negocio
{
    public interface IMaterialApoioNegocio : IBaseNegocio<MaterialApoioViewModel, MaterialApoio>
    {
        IList<MaterialApoioViewModel> BuscarMaterial();
        void CriarMaterial(MaterialApoioViewModel viewModel, Guid idUsuarioLogado);
        void EditarMaterial(MaterialApoioViewModel editar, Guid idUsuarioLogado);
    }
}
