using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;

namespace MMN.INegocio.Negocio
{
    public interface IMensagemNegocio : IBaseNegocio<MensagemViewModel, Mensagem>
    {
        bool EnviarNotificacao(Guid IdUsuario, string titulo, string mensagem);
        IList<MensagemViewModel> BuscarComunicados();
    }
}
