using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Negocio.Negocio
{
    public class TutorialNegocio : BaseNegocio<TutorialViewModel, Tutorial>, ITutorialNegocio
    {
        private readonly ITutorialRepositorio _repositorio;
        private readonly IMapper _mapper;

        public TutorialNegocio(ITutorialRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
        public void CriarTutorial(TutorialViewModel viewModel)
        {
            var dadosTutorial = new Tutorial
            {
                Nome = viewModel.Nome,
                Descricao = viewModel.Descricao,
                Ativo = viewModel.Ativo,
                URL = viewModel.URL,
                DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
            };

            _repositorio.Insert(dadosTutorial);
            _repositorio.SaveChanges();
        }

        public void EditarTutorial(TutorialViewModel editar)
        {
            var dadosTutorial = _repositorio.GetById(editar.IdTutorial);


            dadosTutorial.Nome = editar.Nome;
            dadosTutorial.Descricao = editar.Descricao;
            dadosTutorial.Ativo = editar.Ativo;
            dadosTutorial.URL = editar.URL;
            dadosTutorial.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();


            _repositorio.Update(dadosTutorial);
            _repositorio.SaveChanges();
        }
    }
}
