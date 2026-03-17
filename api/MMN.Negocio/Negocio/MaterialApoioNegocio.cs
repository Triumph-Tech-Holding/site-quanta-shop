using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class MaterialApoioNegocio : BaseNegocio<MaterialApoioViewModel, MaterialApoio>, IMaterialApoioNegocio
    {
        private readonly IMaterialApoioRepositorio _repositorio;
        private readonly IMapper _mapper;

        public MaterialApoioNegocio(IMaterialApoioRepositorio repositorio, IMapper mapper) : base (repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public IList<MaterialApoioViewModel> BuscarMaterial()
        {
            return _mapper.Map<List<MaterialApoioViewModel>>(_repositorio.BuscarMaterial());
        }

        public void CriarMaterial(MaterialApoioViewModel viewModel, Guid idUsuarioLogado)
        {
            var dadosMaterial = new MaterialApoio
            {
                Nome = viewModel.Nome,
                Descricao = viewModel.Descricao,
                Ativo = viewModel.Ativo,
                URLMaterial = viewModel.URLMaterial,
                DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
            };

            _repositorio.Insert(dadosMaterial);
            _repositorio.SaveChanges();
        }

        public void EditarMaterial(MaterialApoioViewModel editar, Guid idUsuarioLogado)
        {
            var item = _repositorio.FirstNoTracking(f => f.IdMaterial == editar.IdMaterial);

            
                var dadosMaterial = _repositorio.GetById(item.IdMaterial);

                if (editar.URLMaterial == null)
                {
                    dadosMaterial.Nome = editar.Nome;
                    dadosMaterial.Descricao = editar.Descricao;
                    dadosMaterial.Ativo = editar.Ativo;
                    dadosMaterial.UltimaAtualizacao = DateTime.UtcNow.HorarioBrasilia();

                    _repositorio.Update(dadosMaterial);
                } 
            
            else 
            {
                
                
                dadosMaterial.Nome = editar.Nome;
                dadosMaterial.Descricao = editar.Descricao;
                dadosMaterial.Ativo = editar.Ativo;
                dadosMaterial.URLMaterial = editar.URLMaterial;
                dadosMaterial.UltimaAtualizacao = DateTime.UtcNow.HorarioBrasilia();

                _repositorio.Update(dadosMaterial);
            }

            
            
            _repositorio.SaveChanges();
        }
    }
}
