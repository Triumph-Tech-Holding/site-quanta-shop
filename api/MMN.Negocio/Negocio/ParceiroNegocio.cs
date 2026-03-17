using AutoMapper;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Afilio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MMN.Negocio.Negocio
{
    public class ParceiroNegocio : BaseNegocio<ParceiroViewModel, Parceiro>, IParceiroNegocio
    {
        private readonly IMapper _mapper;
        private readonly IParceiroRepositorio _repositorio;

        public ParceiroNegocio(IMapper mapper, IParceiroRepositorio repositorio) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public Parceiro CriarParceiro(ParceiroViewModel parceiro)
        {
            var parceiros = _repositorio.Get(p => p.IdCredenciado == parceiro.IdCredenciado);
            if (parceiros.Any(p => p.Celular == parceiro.Celular))
            {
                throw new PadraoException("parceiro_numero_em_uso");
            }

            if (parceiros.Any(p => p.Nome == parceiro.Celular))
            {
                throw new PadraoException("parceiro_nome_em_uso");
            }

            if (parceiro.Celular.Length > 15)
            {
                throw new PadraoException("telefone_invalido");
            }

            _repositorio.Insert(new Parceiro
            {
                Ativo = true,
                DataCriacao = DateTime.UtcNow.HorarioBrasilia(),
                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                Celular = parceiro.Celular,
                IdCredenciado = parceiro.IdCredenciado,
                Descricao = parceiro.Descricao,
                Nome = parceiro.Nome
            });

            _repositorio.SaveChanges();

            return _repositorio.FirstNoTracking(p => p.Nome == parceiro.Nome && p.Celular == parceiro.Celular);
        }

        public Parceiro AtualizarParceiro(ParceiroViewModel parceiro)
        {
            var parceiros = _repositorio.Get(p => p.IdCredenciado == parceiro.IdCredenciado);
            if (parceiros.Any(p => p.IdParceiro != parceiro.IdParceiro && p.Celular == parceiro.Celular))
            {
                throw new PadraoException("parceiro_numero_em_uso");
            }

            if (parceiros.Any(p => p.IdParceiro != parceiro.IdParceiro && p.Nome == parceiro.Celular))
            {
                throw new PadraoException("parceiro_nome_em_uso");
            }

            if (parceiro.Celular.Length > 15)
            {
                throw new PadraoException("telefone_invalido");
            }

            var parceiroBanco = _repositorio.FirstNoTracking(p => p.IdParceiro == parceiro.IdParceiro);
            parceiroBanco.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();
            parceiroBanco.Nome = parceiro.Nome;
            parceiroBanco.Celular = parceiro.Celular;
            parceiroBanco.Ativo = parceiro.Ativo;
            parceiroBanco.Descricao = parceiro.Descricao;

            _repositorio.Update(parceiroBanco);

            _repositorio.SaveChanges();

            return parceiroBanco;
        }

        public List<Parceiro> ObterParceiros(Guid idUsuario)
        {
            return _repositorio.Get(p => p.IdCredenciado == idUsuario).ToList();
        }

        public Parceiro ObterParceiro(int idParceiro)
        {
            return _repositorio.FirstNoTracking(p => p.IdParceiro == idParceiro);
        }
    }
}
