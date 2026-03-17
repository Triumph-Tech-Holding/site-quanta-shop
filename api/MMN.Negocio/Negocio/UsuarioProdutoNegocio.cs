using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using System;

namespace MMN.Negocio.Negocio
{
    public class UsuarioProdutoNegocio : BaseNegocio<UsuarioProdutoViewModel, UsuarioProduto>, IUsuarioProdutoNegocio
    {
        private readonly IUsuarioProdutoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public UsuarioProdutoNegocio(IUsuarioProdutoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public UsuarioProdutoViewModel BuscarPorPedido(long IdPedido)
        {
            return _mapper.Map<UsuarioProdutoViewModel>(_repositorio.BuscarPorPedido(IdPedido));
        }

        public UsuarioProdutoViewModel BuscarProdutoAtivo(Guid idUsuario)
        {
            return _mapper.Map<UsuarioProdutoViewModel>(_repositorio.BuscarProdutoAtivo(idUsuario));
        }

        public UsuarioProdutoViewModel BuscarAssinaturaAtiva(Guid idUsuario)
        {
            return _mapper.Map<UsuarioProdutoViewModel>(_repositorio.BuscarAssinaturaAtiva(idUsuario));
        }


        public bool InserirPlanoManual(UsuarioViewModel usuario, ProdutoViewModel plano, int planoAtivoId)
        {
            return _repositorio.InserirPlanoManual(_mapper.Map<Usuario>(usuario), _mapper.Map<Produto>(plano), planoAtivoId);
        }

        public bool InserirPlanoPresente(UsuarioViewModel usuario, ProdutoViewModel plano, int planoAtivoId)
        {
            return _repositorio.InserirPlanoPresente(_mapper.Map<Usuario>(usuario), _mapper.Map<Produto>(plano), planoAtivoId);
        }
    }
}
