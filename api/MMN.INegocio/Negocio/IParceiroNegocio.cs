using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
namespace MMN.INegocio.Negocio
{
    public interface IParceiroNegocio : IBaseNegocio<ParceiroViewModel, Parceiro>
    {
        public List<Parceiro> ObterParceiros(Guid idUsuario);
        public Parceiro ObterParceiro(int idParceiro);
        public Parceiro CriarParceiro(ParceiroViewModel parceiro);
        public Parceiro AtualizarParceiro(ParceiroViewModel parceiro);
    }
}
