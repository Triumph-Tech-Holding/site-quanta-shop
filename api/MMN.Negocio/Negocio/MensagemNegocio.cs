using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;

namespace MMN.Negocio.Negocio
{
    public class MensagemNegocio : BaseNegocio<MensagemViewModel, Mensagem>, IMensagemNegocio
    {
        private IMensagemRepositorio _repositorio;
        private IMapper _mapper;

        public MensagemNegocio(IMensagemRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public IList<MensagemViewModel> BuscarComunicados()
        {
            return _mapper.Map<IList<MensagemViewModel>>(_repositorio.BuscarComunicados());
        }

        public bool EnviarNotificacao(Guid IdUsuario, string titulo, string mensagem)
        {
            try
            {
                Insert(new MensagemViewModel
                {
                    DataCadastro = DateTime.Now.HorarioBrasilia(),
                    TipoMensagem = TipoMensagem.Aviso,
                    Ativo = true,
                    Titulo = titulo,
                    Texto = mensagem,
                    IdUsuarioDestino = IdUsuario
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
