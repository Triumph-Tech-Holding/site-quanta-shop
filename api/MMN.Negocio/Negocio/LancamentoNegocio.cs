using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Enum;
using MMN.Util.Extensions;

namespace MMN.Negocio.Negocio
{
    public class LancamentoNegocio : BaseNegocio<LancamentoViewModel, Lancamento>, ILancamentoNegocio
    {
        private readonly ILancamentoRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ITipoNegocio _tipoNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;

        public LancamentoNegocio(ILancamentoRepositorio repositorio,
            IMapper mapper,
            ITipoNegocio tipoNegocio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IUsuarioNegocio usuarioNegocio) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _tipoNegocio = tipoNegocio;
            _usuarioNegocio = usuarioNegocio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
        }

        public IList<LancamentoViewModel> BuscarPorIdUsuario(FiltroViewModel.BuscarExtrato model, Guid idUsuario)
        {
            return _mapper.Map<List<LancamentoViewModel>>(_repositorio.BuscarPorIdUsuario(model, idUsuario));
        }

        public object ObterSaldoPorTipo(Guid idUsuario)
        {
            var tipoCashback = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("CHBK")).IdTipo;
            var tipoResidualCashback = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("DTCSH")).IdTipo;
            var tipoCashbackDobrado = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("BDOB")).IdTipo;
            var tipoResidualCashbackDobrado = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("BRCD")).IdTipo;
            var tipoCompressaoDinamica = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("BDIN")).IdTipo;
            var tipoCashbackCredenciado = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("CHBLF")).IdTipo;
            var tipoResidualCashbackCredenciado = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("DCPLJ")).IdTipo;
            var tipoBonusAdesao = _tipoNegocio.GetFromCache().FirstOrDefault(w => w.Chave.Equals("DTBAD")).IdTipo;
            var tipoBonusResidualAdesao = _tipoNegocio.GetFromCache().FirstOrDefault(w => w.Chave.Equals("DTAD")).IdTipo;
            var tipoBonusCredenciamento = _tipoNegocio.GetFromCache().FirstOrDefault(w => w.Chave.Equals("PPOC")).IdTipo;
            var tipoBonusResidualCredenciamento = _tipoNegocio.GetFromCache().FirstOrDefault(w => w.Chave.Equals("DPPOC")).IdTipo;
            var tipoSaque = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("SQ")).IdTipo;
            var lancamentos = Get(w => w.Ativo && w.IdUsuario == idUsuario, "LancamentoRetido").ToList();

            var valorCashback = lancamentos.Where(w => w.IdTipo == tipoCashback || w.IdTipo == tipoCashbackCredenciado).Sum(s => s.Valor);
            var valorCashbackDobrado = lancamentos.Where(w =>w.IdTipo == tipoCashbackDobrado).Sum(s => s.Valor);
            valorCashbackDobrado += lancamentos.Where(w => w.IdTipo == tipoResidualCashbackDobrado).Sum(s => s.Valor);
            var valorBonusCompressaoDinamica = lancamentos.Where(w => w.IdTipo == tipoCompressaoDinamica).Sum(s => s.Valor);
            var valorResidualCashback = lancamentos.Where(w => w.IdTipo == tipoResidualCashback || w.IdTipo == tipoResidualCashbackCredenciado).Sum(s => s.Valor);
            var valorAdesao = lancamentos.Where(w => w.IdTipo == tipoBonusAdesao).Sum(s => s.Valor);
            var valorResidualAdesao = lancamentos.Where(w => w.IdTipo == tipoBonusResidualAdesao).Sum(s => s.Valor);
            var valorBonusCredenciamento = lancamentos.Where(w => w.IdTipo == tipoBonusCredenciamento).Sum(s => s.Valor);
            var valorBonusResidualCredenciamento = lancamentos.Where(w => w.IdTipo == tipoBonusResidualCredenciamento).Sum(s => s.Valor);

            string[] array = new string[3] { "LM", "EST", "PLCD" };
            return new
            {
                valorCashback,
                valorResidualCashback,
                valorCashbackDobrado,
                valorBonusCompressaoDinamica,
                valorAdesao,
                valorResidualAdesao,
                valorBonusCredenciamento,
                valorBonusResidualCredenciamento,
                totalEntradas = lancamentos.Where(w => w.Valor > 0).Sum(s => s.Valor),
                totalSaidas = lancamentos.Where(w => w.Valor < 0).Sum(s => s.Valor),
                bloqueado = lancamentos.Where(w => w.Valor > 0 && w.Bloqueado).Sum(s => s.Valor),
                valorSaque = lancamentos.Where(w => w.IdTipo == tipoSaque).Sum(s => s.Valor),
                outrasSaidas = lancamentos.Where(w => w.IdTipo != tipoSaque && w.Valor < 0).Sum(s => s.Valor),
                outrasEntradas = lancamentos.Where(w =>
                    w.IdTipo != tipoCashback &&
                    w.IdTipo != tipoResidualCashback &&
                    w.IdTipo != tipoCashbackCredenciado &&
                    w.IdTipo != tipoResidualCashbackCredenciado &&
                    w.IdTipo != tipoBonusAdesao &&
                    w.IdTipo != tipoBonusResidualAdesao &&
                    w.IdTipo != tipoBonusCredenciamento &&
                    w.IdTipo != tipoBonusResidualCredenciamento &&
                    w.Valor > 0).Sum(s => s.Valor),
                outrosLancamentos = _tipoNegocio.GetFromCache().Where(w => array.Contains(w.Chave)).ToList(),
                saldoRetido = lancamentos.Sum(l => l.LancamentoRetido.Where(w => w.Ativo).Sum(lt => lt.Valor))
            };
        }

        public object GerarLancamentoManual(LancamentoManualViewModel viewModel, Guid idUsuario)
        {
            var usuarioLogado = _usuarioNegocio.First(f => f.IdUsuario == idUsuario);
            var usuario = _usuarioNegocio.First(f => f.Login.Equals(viewModel.Login));

            if (usuario != null && usuarioLogado != null)
            {
                var usuarioProduto = _usuarioProdutoNegocio.Last(f => f.IdUsuario.Equals(usuario.IdUsuario));
                usuario.ProdutoAtivo = usuarioProduto;
                var nivel = 0;
                if (usuarioProduto != null)
                    nivel = usuarioProduto.IdProduto == 1 ? 0 : usuarioProduto.IdProduto == 2 ? 3 :
                            usuarioProduto.IdProduto == 3 ? 4 : usuarioProduto.IdProduto == 4 ? 5 :
                            usuarioProduto.IdProduto == 5 ? 6 : usuarioProduto.IdProduto == 6 ? 12 : 0;

                var listUsuariosPatrocinadores = _usuarioNegocio.ListaUsuariosPatrocinadores(usuario.IdUsuario, nivel);
                _repositorio.GerarLancamentoManual(usuario, viewModel, usuarioLogado, listUsuariosPatrocinadores);
                return new { message = "Lançamento manual gerado com sucesso!" };
            }

            throw new PadraoException("usuario_nao_encontrado");
        }

        public static LancamentoViewModel GerarLancamentoSaldo(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new Exception("Pedido não informado");
            }

            if (pedido.UsuarioProduto.Count() != 1 ||
                pedido.UsuarioProduto.Any(a => a.Produto.IdProduto != 10))
            {
                throw new Exception("Deve ser usado um pedido de saldo.");
            }


            var lancamentoSaldo = new LancamentoViewModel
            {
                Ativo = true,
                DataLancamento = DateTime.UtcNow.HorarioBrasilia(),
                Descricao = $"Compra de saldo. Usuário: {pedido.Usuario.Login}",
                IdStatus = (int)StatusTransacaoEnum.Finalizada,
                IdTipo = pedido.Transacao.IdTipo,
                IdUsuario = pedido.IdUsuario,
                IdTransacao = pedido.IdTransacao.Value,
                Valor = pedido.ValorPedido - pedido.ValorTaxa
            };

            return lancamentoSaldo;
        }

        public LancamentoViewModel GerarLancamentoTaxaSaldo(Pedido pedido)
        {
            var tipoTaxaSaldo = _tipoNegocio.GetFromCache().FirstOrDefault(f => f.Chave.Equals("DTSLD")).IdTipo;
            if (pedido == null)
            {
                throw new Exception("Pedido não informado");
            }

            if (pedido.UsuarioProduto.Count() != 1 ||
                pedido.UsuarioProduto.Any(a => a.Produto.IdProduto != 10))
            {
                throw new Exception("Deve ser usado um pedido de saldo.");
            }


            var lancamentoTaxaSaldo = new LancamentoViewModel
            {
                Ativo = true,
                DataLancamento = DateTime.UtcNow.HorarioBrasilia(),
                Descricao = $"Taxa de compra de saldo. Usuário: {pedido.Usuario.Login}",
                IdStatus = (int)StatusTransacaoEnum.Finalizada,
                IdTipo = tipoTaxaSaldo,
                IdUsuario = new Guid("30C0BF78-879A-47C1-9869-D485B8D84E0B"),
                IdTransacao = pedido.IdTransacao.Value,
                Valor = pedido.ValorTaxa
            };

            return lancamentoTaxaSaldo;
        }

        public IList<LancamentoViewModel> ObterLancamentos(Guid idUsuario, int tipoLancamento)
        {
            var tipoCashback = "CHBK";
            var tipoCashbackDobrado = "BDOB";
            var tipoCompressaoDinamica = "BDIN";
            var tipoResidualCashback = "DTCSH";
            var tipoCashbackCredenciado = "CHBLF";
            var tipoResidualCashbackCredenciado = "DCPLJ";
            var tipoBonusAdesao = "DTBAD";
            var tipoBonusResidualAdesao = "DTAD";
            var tipoBonusCredenciamento = "PPOC";
            var tipoBonusResidualCredenciamento = "DPPOC";
            var tipoSaque = "SQ";

            switch (tipoLancamento)
            {
                case 1:
                    //cashback
                    return Get(w =>
                            (w.Tipo.Chave == tipoCashback || w.Tipo.Chave == tipoCashbackCredenciado) &&
                            w.IdUsuario == idUsuario)
                        .ToList();
                case 2:
                    //residual cashback
                    return Get(w =>
                            (w.Tipo.Chave == tipoResidualCashback || w.Tipo.Chave == tipoResidualCashbackCredenciado) &&
                            w.IdUsuario == idUsuario)
                        .ToList();
                case 3:
                    //bonus adesao
                    return Get(w =>
                            w.Tipo.Chave == tipoBonusAdesao &&
                            w.IdUsuario == idUsuario)
                        .ToList();
                case 4:
                    //bonus residual adesao
                    return Get(w =>
                            w.Tipo.Chave == tipoBonusResidualAdesao &&
                            w.IdUsuario == idUsuario)
                    .ToList();
                case 5:
                    //outras entradas
                    return Get(w =>
                            w.Tipo.Chave != tipoCashback &&
                            w.Tipo.Chave != tipoResidualCashback &&
                            w.Tipo.Chave != tipoCashbackCredenciado &&
                            w.Tipo.Chave != tipoResidualCashbackCredenciado &&
                            w.Tipo.Chave != tipoBonusAdesao &&
                            w.Tipo.Chave != tipoBonusResidualAdesao &&
                            w.Tipo.Chave != tipoBonusCredenciamento &&
                            w.Tipo.Chave != tipoBonusResidualCredenciamento &&
                            w.Valor > 0 &&
                            w.IdUsuario == idUsuario)
                        .ToList();
                case 6:
                    //saque
                    return Get(w =>
                            w.Tipo.Chave == tipoSaque &&
                            w.IdUsuario == idUsuario)
                        .ToList();
                case 7:
                    //outras saidas
                    return Get(w =>
                        w.Tipo.Chave != tipoSaque &&
                        w.Valor < 0 &&
                        w.IdUsuario == idUsuario)
                    .ToList();
                case 8:
                    //bonus credenciamento
                    return Get(w =>
                        w.Tipo.Chave == tipoBonusCredenciamento &&
                        w.IdUsuario == idUsuario)
                    .ToList();
                case 9:
                    //bonus residual credenciamento
                    return Get(w =>
                        w.Tipo.Chave == tipoBonusResidualCredenciamento &&
                        w.IdUsuario == idUsuario)
                    .ToList();
                case 64:
                    //bonus cashback dobrado
                    return Get(w =>
                        w.Tipo.Chave == tipoCashbackDobrado &&
                        w.IdUsuario == idUsuario)
                    .ToList();
                case 65:
                    //bonus compresao dinamica
                    return Get(w =>
                        w.Tipo.Chave == tipoCompressaoDinamica &&
                        w.IdUsuario == idUsuario)
                    .ToList();
                default:
                    return null;
            }
        }

        public object ObterCashbackDetalahdo()
        {
            var lancamentos = Get(l => l.Ativo && l.IdStatus == (int)StatusTransacaoEnum.Finalizada && (l.Tipo.Chave == "CHBK" || l.Tipo.Chave == "DTCSH"),
                "Tipo",
                "Usuario");

            return new
            {
                total = lancamentos.Sum(s => s.Valor),
                cashback = lancamentos.Where(w => w.Tipo.Chave == "CHBK").Sum(s => s.Valor),
                cashbackResidual = lancamentos.Where(w => w.Tipo.Chave == "DTCSH" && w.Usuario.IdUsuario != Guid.Parse("30C0BF78-879A-47C1-9869-D485B8D84E0B")).Sum(s => s.Valor),
                bigcash = lancamentos.Where(w => w.Usuario.IdUsuario == Guid.Parse("30C0BF78-879A-47C1-9869-D485B8D84E0B")).Sum(s => s.Valor)
            };
        }

        public object ObterLancamentosAdmin(FiltroViewModel.FiltroLancamento filtros, Guid idUsuario)
        {
            var query = GetIEnumerable(w => true, "Usuario");

            if (!string.IsNullOrWhiteSpace(filtros.IdTransacao))
            {
                long idTransacao;

                Int64.TryParse(filtros.IdTransacao, out idTransacao);

                query = query.Where(w => w.IdTransacao == idTransacao);
            }

            if (!string.IsNullOrWhiteSpace(filtros.Login))
            {
                var loginFiltro = filtros.Login.ToLower();
                query = query.Where(w => w.Usuario.Login.ToLower().Contains(loginFiltro));
            }

            if (!string.IsNullOrWhiteSpace(filtros.Nome))
            {
                var nomeFiltro = filtros.Nome.ToLower();
                query = query.Where(w => w.Usuario.Nome.ToLower().Contains(nomeFiltro));
            }

            int totalDeLinhas = query.ToList().Count;

            var listaOrdenada = query.OrderByDescending(x => x.IdTransacao).ThenByDescending(x => x.DataLancamento).ThenByDescending(o => o.Valor).ToList();

            int skipRows = (filtros.Pagina - 1) * filtros.PorPagina;

            var resultado = listaOrdenada.Select(s => new
            {
                idTransacao = s.IdTransacao,
                nome = s.Usuario.Nome.ToUpper(),
                login = s.Usuario.Login.ToLower(),
                dataLancamento = s.DataLancamento,
                descricao = s.Descricao,
                valor = s.Valor.ToString("C2")
            })
            .Skip(skipRows)
            .Take(filtros.PorPagina)
            .ToList();

            return resultado;
        }
    }
}