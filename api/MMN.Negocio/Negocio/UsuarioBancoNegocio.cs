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

namespace MMN.Negocio.Negocio
{
    public class UsuarioBancoNegocio : BaseNegocio<UsuarioBancoViewModel, UsuarioBanco>, IUsuarioBancoNegocio
    {
        private readonly IUsuarioBancoRepositorio _repositorio;
        private readonly IMapper _mapper;
        public UsuarioBancoNegocio(IUsuarioBancoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public bool CadastrarUsuarioBanco(UsuarioBancoViewModel viewModel, Guid IdUsuario)
        {
            viewModel.IdUsuario = IdUsuario;
            viewModel.Ativo = true;
            Insert(viewModel);

            return true;
        }
    }
}
