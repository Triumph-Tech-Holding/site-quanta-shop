using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Negocio.Negocio
{
    public class FaqNegocio : BaseNegocio<FaqViewModel, Faq>, IFaqNegocio
    {
        private readonly IFaqRepositorio _repositorio;
        private readonly IMapper _mapper;

        public FaqNegocio(IFaqRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public IList<FaqViewModel> BuscarFaq()
        {
            return _mapper.Map<List<FaqViewModel>>(_repositorio.BuscarFaq());
        }

        public void CriarFaq(FaqViewModel viewModel, Guid idUsuarioLogado)
        {
            var dadosFaq = new Faq
            {
                Pergunta = viewModel.Pergunta,
                Resposta = viewModel.Resposta,
                Ativo = viewModel.Ativo,
                DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
            };

            _repositorio.Insert(dadosFaq);
            _repositorio.SaveChanges();
        }

        public void EditarFaq(FaqViewModel editar, Guid idUsuarioLogado)
        {
            var dadosFaq = _repositorio.GetById(editar.IdFaq);

            dadosFaq.Pergunta = editar.Pergunta;
            dadosFaq.Resposta = editar.Resposta;
            dadosFaq.Ativo = editar.Ativo;
            dadosFaq.UltimaAtualizacao = DateTime.UtcNow.HorarioBrasilia();

            _repositorio.Update(dadosFaq);
            _repositorio.SaveChanges();
        }

        public List<Faq> FiltrarFAQ(FiltroFaqAdmin filtroFaqAdmin, Guid idUsuarioLogado)
        {
            var filtro = _repositorio.GetNoTracking(f => 
            (!string.IsNullOrEmpty(filtroFaqAdmin.Pergunta) ? f.Pergunta.ToLower().Contains(filtroFaqAdmin.Pergunta.ToLower()) : true ||
            !string.IsNullOrEmpty(filtroFaqAdmin.Resposta) ? f.Resposta.ToLower().Contains(filtroFaqAdmin.Resposta.ToLower()) : true) &&
            (filtroFaqAdmin.IdStatus ?? f.Ativo)== f.Ativo)

            .ToList();

            return (filtro);
        }
    }
}
