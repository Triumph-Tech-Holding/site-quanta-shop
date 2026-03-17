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
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class SaqueNegocio : BaseNegocio<SaqueViewModel, Saque>, ISaqueNegocio
    {
        private readonly ISaqueRepositorio _repositorio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IProceduresRepositorio _repositorioProcedure;
        private readonly IMapper _mapper;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly ICupomCashbackNegocio _cuponCashbackNegocio;
        public SaqueNegocio(ISaqueRepositorio saqueRepositorio, IMapper mapper, IUsuarioNegocio usuarioNegocio, IProceduresRepositorio proceduresRepositorio, IPedidoNegocio pedidoNegocio = null, ICupomCashbackNegocio cuponCashbackNegocio = null) : base(saqueRepositorio, mapper)
        {
            _repositorio = saqueRepositorio;
            _mapper = mapper;
            _usuarioNegocio = usuarioNegocio;
            _repositorioProcedure = proceduresRepositorio;
            _pedidoNegocio = pedidoNegocio;
            _cuponCashbackNegocio = cuponCashbackNegocio;
        }

        public void AlterarStatusSaque(List<SaqueViewModel> listaIdsSaque, StatusTransacaoEnum status)
        {
            foreach (var item in listaIdsSaque)
            {
                var saque = _repositorio.GetById(item.IdSaque);
                saque.IdStatus = (int)status;
                _repositorio.Update(saque);
                _repositorio.SaveChanges();
            }
        }

        public void AprovarSolicitacaoSaque(List<SaqueViewModel> lista, Guid idUsuarioLogado)
        {
            var dtAprovacao = DateTime.Now.HorarioBrasilia();
            var usuarioLogado = _usuarioNegocio.GetById(idUsuarioLogado);
            foreach (var item in lista)
            {
                var saque = _repositorio.GetById(item.IdSaque);
                saque.IdStatus = (int)StatusTransacaoEnum.Finalizada;
                saque.Historico += $"Saque aprovado pelo usuário {usuarioLogado.Login} dia {dtAprovacao:dd/MM/yyyy HH:mm} <br/><br/>";
                saque.DataAprovacao = dtAprovacao;
                saque.Aprovador = usuarioLogado.Login;
                _repositorio.Update(saque);
                _repositorio.SaveChanges();
            }
        }

        public object BuscarPagamentos(FiltroViewModel.FiltroSaque model)
        {
            var totalPaginas = 0;
            var lista = _repositorio.BuscarPagamentos(model, out totalPaginas);
            return new
            {
                totalPaginas,
                lista = lista.Select(s => new
                {
                    s.DataSolicitacao,
                    s.DataProcessado,
                    s.EnderecoBTC,
                    s.IdSaque,
                    Status = s.Status.Nome,
                    s.Usuario.Login,
                    s.Usuario.Nome,
                    s.Valor,
                    ValorTaxa = (s.Valor * s.TaxaSaque),
                    ValorReal = s.Valor - (s.Valor * s.TaxaSaque),
                    s.Processado,
                    TaxaSaque = s.TaxaSaque * 100,
                    Tipo = s.Tipo.Descricao,
                    s.Status.IdStatus,
                    s.Aprovador,
                    s.Historico,
                    s.DataAprovacao
                }).ToList()
            };
        }

        public void CancelarSaque(List<SaqueViewModel> lista)
        {
            _repositorio.CancelarSaque(lista);
        }

        public SaqueViewModel InserirPedidoSaque(SaqueViewModel viewModel)
        {
            return _mapper.Map<SaqueViewModel>(_repositorio.InserirSaque(_mapper.Map<Saque>(viewModel)));
        }

        public ResumoSaqueViewModel ObterResumoSaque(DateTime dataInicio, DateTime dataFim)
        {
            return _repositorioProcedure.spc_ResumoSaque(dataInicio, dataFim);
        }

        public async Task<decimal> ObterConsumoSaque(Guid IdUsuario, DateTime dataInicio, DateTime dataFim)
        {
            var consumoOnline = _pedidoNegocio.Get(p => p.IdUsuario == IdUsuario && p.DataPedido.Date >= dataInicio && p.DataPedido.Date <= dataFim && p.IdAwinTransaction != null).Sum(p => p.ValorPedido);

            var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == IdUsuario);

            var cupomCashbacks = await _cuponCashbackNegocio.GetAsync(cc => cc.Documento == usuario.Documento && cc.DataCompra >= dataInicio.Date && cc.DataCompra.Date <= dataFim);

            var consumoLocal = cupomCashbacks.Sum(cl => cl.Valor);

            return (consumoLocal + consumoOnline);
            
        }

        public async Task<Dictionary<Guid, decimal>> ObterConsumoSaqueBatch(List<Guid> idsUsuarios, DateTime dataInicio, DateTime dataFim)
        {
            if (idsUsuarios == null || !idsUsuarios.Any())
                return new Dictionary<Guid, decimal>();

            var resultado = new Dictionary<Guid, decimal>();

            // Buscar consumo online para todos os usuários de uma vez
            var consumosOnline = _pedidoNegocio
                .Get(p => idsUsuarios.Contains(p.IdUsuario) && 
                         p.DataPedido >= dataInicio && 
                         p.DataPedido <= dataFim && 
                         p.IdAwinTransaction != null)
                .GroupBy(p => p.IdUsuario)
                .Select(g => new { IdUsuario = g.Key, Total = g.Sum(p => p.ValorPedido) })
                .ToList();

            // Buscar usuários e seus documentos
            var usuarios = _usuarioNegocio
                .Get(u => idsUsuarios.Contains(u.IdUsuario))
                .Select(u => new { u.IdUsuario, u.Documento })
                .ToList();

            var documentos = usuarios.Select(u => u.Documento).ToList();

            // Buscar cupons de cashback
            var cupomCashbacks = await _cuponCashbackNegocio.GetAsync(cc => 
                documentos.Contains(cc.Documento) && 
                cc.DataCompra >= dataInicio.Date && 
                cc.DataCompra.Date <= dataFim);

            var consumosLocais = cupomCashbacks
                .GroupBy(cc => cc.Documento)
                .Select(g => new { Documento = g.Key, Total = g.Sum(cl => cl.Valor) })
                .ToList();

            // Montar resultado
            foreach (var idUsuario in idsUsuarios)
            {
                var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
                var consumoOnline = consumosOnline.FirstOrDefault(c => c.IdUsuario == idUsuario)?.Total ?? 0;
                var consumoLocal = usuario != null 
                    ? consumosLocais.FirstOrDefault(c => c.Documento == usuario.Documento)?.Total ?? 0 
                    : 0;

                resultado[idUsuario] = consumoOnline + consumoLocal;
            }

            return resultado;
        }
    }
}
