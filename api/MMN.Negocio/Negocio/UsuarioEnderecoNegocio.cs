using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Base;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Util;

namespace MMN.Negocio.Negocio
{
    public class UsuarioEnderecoNegocio : BaseNegocio<UsuarioEnderecoViewModel, UsuarioEndereco>, IUsuarioEnderecoNegocio
    {

        private readonly IUsuarioEnderecoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public UsuarioEnderecoNegocio(IUsuarioEnderecoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public bool CadastrarEndereco(UsuarioEnderecoViewModel model)
        {
            try
            {
                var usuariEndereco = _mapper.Map<UsuarioEndereco>(model);

                _repositorio.Insert(usuariEndereco);
                _repositorio.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogException("CadastrarEndereco(UsuarioEnderecoViewModel model)", e, "CadastrarEndereco");
                return false;
            }

        }

        public bool EditarEndereco(UsuarioEnderecoViewModel model)
        {
            try
            {
                _repositorio.Update(_mapper.Map<UsuarioEndereco>(model));
                _repositorio.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogException("EditarEndereco(UsuarioEnderecoViewModel model)", e, "CadastrarEndereco");
                return false;
            }
        }

        public UsuarioEnderecoViewModel ObterEnderecoCompleto(Guid idUsuario)
        {
            return _mapper.Map<UsuarioEnderecoViewModel>(_repositorio.ObterEnderecoCompleto(idUsuario));
        }
    }
}
