using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Extensions;
using MMN.Util.Util;

namespace MMN.Negocio.Negocio
{
    public class ProdutoNegocio : BaseNegocio<ProdutoViewModel, Produto>, IProdutoNegocio
    {
        private readonly IProdutoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public ProdutoNegocio(IProdutoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }


        public IList<ProdutoViewModel> BuscarAtivos()
        {
            return _mapper.Map<List<ProdutoViewModel>>(_repositorio.BuscarAtivos());
        }

        public IList<ProdutoViewModel> BuscarTodos()
        {
            return _mapper.Map<List<ProdutoViewModel>>(_repositorio.BuscarTodos());
        }

        public void CriarPlano(ProdutoViewModel viewModel, Guid idUsuarioLogado)
        {
            var dadosPlano = new Produto
            {
                Nome = viewModel.Nome,
                IdCategoria = 2,
                Valor = viewModel.Valor,
                Ativo = viewModel.Ativo,
                Pontos = viewModel.Pontos,
                Parcelas = viewModel.Parcelas,
                DataAutalizacao = DateTime.UtcNow.HorarioBrasilia(),
                Visivel = viewModel.Visivel,
                ImagemUrl = string.Empty,
            };

            

            _repositorio.Insert(dadosPlano);
            _repositorio.SaveChanges();

        }

        public void EditarPlano(ProdutoViewModel editar, Guid IdUsuarioLogado)
        {
            var dadosPlano = _repositorio.GetById(editar.IdProduto);

            var texto = FormatarCampos(dadosPlano, editar);

            dadosPlano.Nome = editar.Nome;
            dadosPlano.Valor = editar.Valor;
            dadosPlano.Ativo = editar.Ativo;
            //dadosPlano.Pontos = editar.Pontos;
            dadosPlano.ReaisPorPonto = editar.ReaisPorPonto;
            dadosPlano.Parcelas = editar.Parcelas;
            dadosPlano.DataAutalizacao = DateTime.UtcNow.HorarioBrasilia();
            dadosPlano.Visivel = editar.Visivel;

            _repositorio.Update(dadosPlano);
            _repositorio.SaveChanges();

            var log = new LogProduto
            {
                IdProduto = editar.IdProduto,
                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                Texto = texto,
                IdUsuario = IdUsuarioLogado
            };

            _repositorio.SalvarLog(log);
        }

        public bool IsPlano(int idProduto)
        {
            return _repositorio
                .Get(f => f.IdProduto == idProduto && f.Categoria.Chave == "INV")
                .Any();
        }

        public IList<LogProduto> ObterHistorico(long id)
        {
            var logs = _repositorio.ObterHistorico(id);

            return logs;
        }

        private string FormatarCampos(Produto dbProduto, ProdutoViewModel novoProduto)
        {
            var texto = $"Alteração feita: \n";

            if(dbProduto.Nome != novoProduto.Nome)
            {
                texto += $"Nome \"{dbProduto.Nome}\" para \"{novoProduto.Nome}\" \n";
            }

            if (dbProduto.Ativo != novoProduto.Ativo)
            {
                var status = dbProduto.Ativo ? "Ativo" : "Inativo";
                var novoStatus = novoProduto.Ativo ? "Ativo" : "Inativo";
                texto += $"Status \"{status}\" para \"{novoStatus}\" \n";
            }

            if (dbProduto.Valor != novoProduto.Valor)
            {
                texto += $"Valor \"{dbProduto.Valor}\" para \"{novoProduto.Valor}\" \n";
            }

            if (dbProduto.Visivel != novoProduto.Visivel)
            {
                texto += $"Visibilidade \"{dbProduto.Visivel}\" para \"{novoProduto.Visivel}\" \n";
            }

            if (dbProduto.Parcelas != novoProduto.Parcelas)
            {
                texto += $"Parcelas \"{dbProduto.Parcelas}\" para \"{novoProduto.Parcelas}\" \n";
            }

            return texto;
        }
    }
}
