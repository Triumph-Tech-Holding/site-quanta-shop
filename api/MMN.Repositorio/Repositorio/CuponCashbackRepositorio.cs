using Dapper;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Repositorio.Repositorio
{
    public class CuponCashbackRepositorio : BaseRepositorio<CupomCashback>, ICuponCashbackRepositorio
    {
        public CuponCashbackRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public async Task<(bool status, string message)> CriarDadosNF(CupomCashbackDadosNF dadosNF)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var cupom = await BuscarPelaChaveDeAcessoAsync(dadosNF.ChaveDeAcesso);
                    var comerciante = await _ctx.Credenciamento.FirstOrDefaultAsync(x => x.Cnpj == dadosNF.CNPJ);

                    if (cupom is null)
                        return (false, "Compra não encontrada");

                    if (comerciante is null)
                        return (false, "Credenciado não encontrado");

                    if (cupom.Status == 10)
                        return (false, "Compra identificada pela chave de acesso anteriormente");

                    cupom.Valor = (decimal)dadosNF.ValorAPagar;
                    cupom.PercentualCashback = comerciante.PercentualCashback.Value;
                    cupom.Documento = dadosNF.CPF;
                    cupom.Descricao = "Cupom escaneado pelo cliente";
                    cupom.MeioPagamento = 19;
                    cupom.IdComerciante = comerciante.IdUsuario.Value;
                    cupom.Status = dadosNF.CPF == "Consumidor nao identificado" || cupom.Documento == dadosNF.CPF ? 10 : 12;

                    await _ctx.AddAsync(dadosNF);
                    await _ctx.SaveChangesAsync();

                    transaction.Commit();

                    return (true, "");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                    return (false, message);
                }
            }
        }

        public async Task<CupomCashback> BuscarPelaChaveDeAcessoAsync(string chaveDeAcesso)
        {
            var sql = $"SELECT * FROM CuponCashback WHERE ChaveDeAcessoNF = @ChaveDeAcesso";
            var parameters = new { ChaveDeAcesso = chaveDeAcesso };

            using var connection = new SqlConnection(ConexaoDataBase.Connection());
            connection.Open();

            return await connection.QueryFirstOrDefaultAsync<CupomCashback>(sql, parameters);
        }

        public async Task<CupomCashback> BuscarPeloTokenAsync(string token)
        {
            var sql = $"SELECT * FROM CuponCashback WHERE Token = @Token";
            var parameters = new { Token = token };

            using var connection = new SqlConnection(ConexaoDataBase.Connection());
            connection.Open();

            return await connection.QueryFirstAsync<CupomCashback>(sql, parameters);
        }

        public async Task<object> ObterCuponsCompraUsuarioCredenciado(FiltroViewModel.FiltroVendasCredenciando viewModel)
        {
            try
            {
                using (var connection = new SqlConnection(ConexaoDataBase.Connection()))
                {
                    var query = @"
                        SELECT 
                            UPPER(CR.Estabelecimento) AS Vendedor,
                            IIF(U.Nome <>  '', UPPER(U.Nome), U.Login) AS Comprador,
                            C.Descricao,
                            C.DataCompra,
                            C.ComprovanteCompra,
                            C.MeioPagamento,
                            C.CompraUsuarioAprovada,
                            C.PercentualCashback,
                            C.Token,
                        C.Valor,
                            C.Valor - (C.Valor * C.PercentualCashback) AS ValorRecebido,
                            C.Valor * C.PercentualCashback AS ValorCashbackUsuario,
                        C.Status
                     FROM 
                         CuponCashback AS C
                     INNER JOIN 
                         Credenciamento AS CR ON C.IdComerciante = CR.IdUsuario
                     INNER JOIN 
                         Usuario AS U ON C.Documento = U.Documento
                     WHERE 
                         CAST(c.DataCompra AS DATE) >= @DataInicio 
                         AND CAST(c.DataCompra AS DATE) <= @DataFim 
                         AND c.IdComerciante = @IdCredenciado 
                         AND (@Login IS NULL OR LOWER(U.Login) LIKE '%' + @Login + '%' COLLATE Latin1_General_CI_AI)
                         AND (@Nome IS NULL OR UPPER(U.Nome) LIKE '%' + @Nome + '%' COLLATE Latin1_General_CI_AI)
                         AND (@Documento IS NULL OR c.Documento = @Documento)
                         AND (@Status IS NULL OR C.Status = @Status)
                         AND (@MeioPagamento IS NULL OR C.MeioPagamento = @MeioPagamento)
                         --AND c.Status <> 9
                         --AND C.Status <> 14
                         --AND C.Status <> 6
                         --AND c.CompraUsuario = 1
                     ORDER BY 
                         c.DataCompra DESC
                     OFFSET 
                         @Offset ROWS 
                     FETCH NEXT 
                         @QuantidadePorPagina ROWS ONLY;

                     SELECT 
                         COUNT(*)
                     FROM 
                         CuponCashback AS C
                     WHERE 
                         CAST(c.DataCompra AS DATE) >= @DataInicio 
                         AND CAST(c.DataCompra AS DATE) <= @DataFim 
                         AND C.IdComerciante = @IdCredenciado 
                         AND C.Status != 9 
                         AND C.Status != 14
                         AND C.Status != 6
                         AND C.CompraUsuario = 0;";

                    var parameters = new
                    {
                        DataInicio = viewModel.DataInicio,
                        DataFim = viewModel.DataFim,
                        IdCredenciado = viewModel.IdCredenciado,
                        Offset = (viewModel.Pagina - 1) * viewModel.QuantidadePorPagina,
                        QuantidadePorPagina = viewModel.QuantidadePorPagina,
                        Nome = viewModel.Nome,
                        Login = viewModel.Login,
                        Documento = viewModel.Documento,
                        Status = viewModel.Situacao,
                        MeioPagamento = viewModel.tipoPagamento
                    };

                    var multiQuery = await connection.QueryMultipleAsync(query, parameters);

                    var cupons = multiQuery.Read().Select(s => new
                    {
                        Vendedor = s.Vendedor,
                        Comprador = s.Comprador,
                        s.Descricao,
                        s.DataCompra,
                        s.ComprovanteCompra,
                        MeioPagamento = ((EnumTipoPagamento)s.MeioPagamento).GetDescription(),
                        s.CompraUsuarioAprovada,
                        s.PercentualCashback,
                        s.Token,
                        s.Valor,
                        ValorRecebido = s.Valor - (s.Valor * s.PercentualCashback),
                        ValorCashbackUsuario = s.Valor * s.PercentualCashback,
                        s.Status
                    }).ToList();

                    var totalRegistros = multiQuery.Read<int>().Single();
                    var totalPages = (int)Math.Ceiling((double)totalRegistros / viewModel.QuantidadePorPagina);

                    return new
                    {
                        totalRegistros,
                        totalPages,
                        data = cupons
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter cupons de compra do usuário credenciado.", ex);
            }
        }


        public void Delete(string key)
        {
            var entity = _ctx.CuponCashback.FirstOrDefault(ccp => ccp.IdCuponCashback == key);
            _ctx.Remove(entity);
        }
    }
}
