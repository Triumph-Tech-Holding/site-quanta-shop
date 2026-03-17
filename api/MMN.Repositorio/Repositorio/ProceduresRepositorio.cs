using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MMN.Repositorio.Repositorio
{
    public class ProceduresRepositorio : IProceduresRepositorio
    {
        protected DatabaseContext _ctx = new DatabaseContext();
        public ProceduresRepositorio(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        private List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                return objT;
            }).ToList();
        }
        private DataTable ExecuteStoredProcedure(string procedureName, ICollection<SqlParameter> parameters = null, IDbContextTransaction transaction = null)
        {
            var table = new DataTable();
            using (var command = _ctx.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = procedureName;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 120;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(new SqlParameter() { ParameterName = param.ParameterName, Value = param.Value });
                    }
                }

                if (transaction != null)
                {
                    command.Transaction = transaction.GetDbTransaction();
                }

                _ctx.Database.OpenConnection();
                var dataReader = command.ExecuteReader();

                table.Load(dataReader);

                dataReader.Close();

                if (transaction == null)
                {
                    _ctx.Database.CloseConnection();
                }
                return table;
            }
        }

        private DataTable ExecuteFunction(string query)
        {
            DataTable dataTable = new DataTable();            
            using (SqlConnection connection = new SqlConnection(_ctx.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }
            }

            return dataTable;
        }

        public BarraDeStatusViewModel spc_obterBarraDeStatus(Guid idUsuario)
        {
            var param = new SqlParameter() { ParameterName = "@idUsuario", Value = idUsuario };
            var table = ExecuteStoredProcedure("spc_obterBarraDeStatus", new List<SqlParameter>() { param });
            return ConvertToList<BarraDeStatusViewModel>(table).FirstOrDefault();
        }

        public PerfilPainelViewModel spc_obterPerfilPainel(Guid idUsuario)
        {
            var param = new SqlParameter() { ParameterName = "@idUsuario", Value = idUsuario };
            var table = ExecuteStoredProcedure("spc_obterPerfilPainel", new List<SqlParameter>() { param });
            return ConvertToList<PerfilPainelViewModel>(table).FirstOrDefault();
        }

        public List<PerformanceRedeViewModel> spc_PerformanceRede(Guid idUsuario, string nome = "", string login = "")
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter() {ParameterName = "@idUsuario", Value = idUsuario },
                new SqlParameter() {ParameterName = "@nome", Value = nome},
                new SqlParameter() {ParameterName = "@login", Value = login},
            };
            var table = ExecuteStoredProcedure("spc_PerformanceRede", parameters);
            return ConvertToList<PerformanceRedeViewModel>(table);
        }

        public List<LancamentoRedeUsuarioViewModel> spc_LancamentoRedeUsuario(Guid idUsuario)
        {
            var param = new SqlParameter() { ParameterName = "@idUsuario", Value = idUsuario };
            var table = ExecuteStoredProcedure("spc_LancamentoRedeUsuario", new List<SqlParameter>() { param });
            return ConvertToList<LancamentoRedeUsuarioViewModel>(table);
        }

        public List<PedidosProcedureViewModel> spc_Pedidos(FiltroViewModel.BuscarPedido filtro, string idUsuario)
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            string id = null;

            if (!string.IsNullOrEmpty(filtro.DataInicio))
                dataIni = Convert.ToDateTime(filtro.DataInicio);

            if (!string.IsNullOrEmpty(filtro.DataFim))
                dataFim = Convert.ToDateTime(filtro.DataFim);

            if (!string.IsNullOrEmpty(idUsuario))
                id = idUsuario;

            if (filtro.IdStatus.Equals(0))
                filtro.IdStatus = null;


            var parameters = new List<SqlParameter>
            {
                new SqlParameter() {ParameterName = "@IdUsuario", Value = id },
                new SqlParameter() {ParameterName = "@DataInicio", Value = dataIni},
                new SqlParameter() {ParameterName = "@DataFim", Value = dataFim},
                new SqlParameter() {ParameterName = "@StatusTransacao", Value = filtro.IdStatus}
            };

            var table = ExecuteStoredProcedure("spc_Pedidos", parameters);

            return ConvertToList<PedidosProcedureViewModel>(table);
        }

        public LimitesGanhosViewModel spc_LimitesGanhos(object idUsuario)
        {
            var param = new SqlParameter() { ParameterName = "@idUsuario", Value = idUsuario };
            var table = ExecuteStoredProcedure("spc_limitesGanhos", new List<SqlParameter>() { param });
            return ConvertToList<LimitesGanhosViewModel>(table).FirstOrDefault();
        }

        public ResumoSaqueViewModel spc_ResumoSaque(DateTime dataInicio, DateTime dataFim)
        {
            var param = new List<SqlParameter>();

            param.Add(new SqlParameter() { ParameterName = "@dataInicio", Value = dataInicio });
            param.Add(new SqlParameter() { ParameterName = "@dataFim", Value = dataFim });
            var table = ExecuteStoredProcedure("spc_ResumoSaque", param);
            return ConvertToList<ResumoSaqueViewModel>(table).FirstOrDefault();
        }

        public void spc_DistribuicaoPlanoAdquirido(long idPedido)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@idPedido", Value = idPedido }
            };

            ExecuteStoredProcedure("spc_DistribuicaoPlanoAdquirido", param);
        }

        public void spc_DistribuicaoParcela(long idPedido, long idPagamento, IDbContextTransaction transaction = null)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@idPedido", Value = idPedido },
                new SqlParameter() { ParameterName = "@idPagamento", Value = idPagamento }
            };

            ExecuteStoredProcedure("spc_DistribuirParcela", param, transaction);
        }

        public void spc_LancarCashback(long idPedido,
            decimal totalCashback,
            IDbContextTransaction transaction = null)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@idPedido", Value = idPedido },
                new SqlParameter() { ParameterName = "@totalCashback", Value = totalCashback },
            };
            ExecuteStoredProcedure("spc_LancarCashback", param, transaction);
        }

        public IList<UsuarioDownLineViewModel> spc_UsuarioDownLine(Guid idUsuario)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@idUsuario", Value = idUsuario }
            };
            var table = ExecuteStoredProcedure("spc_UsuarioDownLine", param);
            return ConvertToList<UsuarioDownLineViewModel>(table);
        }

        public PontuacaoPorValorViewModel spc_GetPontuacaoUsuarioPorValor(Guid idUsuario)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@IdUsuario", Value = idUsuario }
            };
            var table = ExecuteStoredProcedure("spc_GetUsuarioPontuacaoPorValor", param);

            var pontuacao = new PontuacaoPorValorViewModel();

            if (!(table.Rows[0][0] is DBNull))
                pontuacao.PontosUsuario = Convert.ToInt32((decimal)table.Rows[0][0]);

            if (!(table.Rows[0][1] is DBNull))
                pontuacao.PontosRedeElegivel = Convert.ToInt32((decimal)table.Rows[0][1]);

            if (!(table.Rows[0][2] is DBNull))
                pontuacao.TotalPontosRede = Convert.ToInt32((decimal)table.Rows[0][2]);


            return pontuacao;
        }

        public decimal spc_ObterPontuacaoUsuarioPremiacao(Guid idUsuario, int porcentagem, int totalPontos, int idGraduacao)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@idusuario", Value = idUsuario },
                new SqlParameter() { ParameterName = "@porcentagem", Value = porcentagem },
                new SqlParameter() { ParameterName = "@totalPontos", Value = totalPontos },
                new SqlParameter() { ParameterName = "@idGraduacaoUsuario", Value = idGraduacao }
            };
            var table = ExecuteStoredProcedure("spc_ObterPontuacaoUsuarioPremiacao", param);

            return (decimal)table.Rows[0][0];
        }

        public IList<RankUsuarioViewModel> spc_ObterRankUsuario(Guid idUsuario)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@IdUsuario", Value = idUsuario }
            };
            var table = ExecuteStoredProcedure("spc_ObterRankUsuario", param);

            return ConvertToList<RankUsuarioViewModel>(table);
        }

        public IList<RankUsuarioViewModel> spc_ObterRankUsuarioFiltrado(Guid idUsuario, string login, string ordenacao)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@IdUsuario", Value = idUsuario },
                new SqlParameter() { ParameterName = "@login", Value = login },
                new SqlParameter() { ParameterName = "@ordenacao", Value = ordenacao }
            };
            var table = ExecuteStoredProcedure("spc_ObterRankUsuarioFiltrado", param);

            return ConvertToList<RankUsuarioViewModel>(table);
        }

        public void spc_AtualizaGraduacao()
        {
            ExecuteStoredProcedure("spc_AtualizaGraduacao");
        }

        public List<RelatorioMensalCashbackViewModel> spc_RelatorioMensalCashback(
            DateTime? dataInicial = null,
            DateTime? datafinal = null)
        {
            var dataInicioParameter = new SqlParameter() { ParameterName = "@DataInicial", Value = dataInicial };
            var datafinalParameter = new SqlParameter() { ParameterName = "@Datafinal", Value = datafinal };
            var table = ExecuteStoredProcedure(
                "spc_RelatorioMensalCashback",
                new List<SqlParameter>() {
                    dataInicioParameter,
                    datafinalParameter
                });

            return ConvertToList<RelatorioMensalCashbackViewModel>(table);
        }

        public void spc_PagamentoComSaldo(Guid idCupomCashback)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@cupomCashbackId", Value = idCupomCashback }
            };

            ExecuteStoredProcedure("spPagamentoComSaldo", param);
        }

        public void sp_ObjetivoIndicacao(Guid idUsuario, IDbContextTransaction transaction = null)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@IdUsuario", Value = idUsuario },                
            };
            ExecuteStoredProcedure("sp_ObjetivoIndicacao", param, transaction);
        }

        public void sp_InsertQuantaAmizade(Guid idUsuario, IDbContextTransaction transaction = null)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@IdUsuario", Value = idUsuario },
            };
            ExecuteStoredProcedure("sp_InsertQuantaAmizade", param, transaction);
        }

        public decimal fnc_ObterResumoConsumoMensal(Guid idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            var table = ExecuteFunction($"SELECT Total FROM dbo.fnc_ObterResumoConsumoMensal('{idUsuario}', '{dataInicial:yyyy-MM-dd}', '{dataFinal:yyyy-MM-dd}')");
            if(table.Rows.Count > 0)
                return (decimal)table.Rows[0][0];

            return 0;
        }

        public void sp_LancarCashback_Assinatura (int usuarioProdutoId, IDbContextTransaction transaction = null)
        {
            var param = new List<SqlParameter>() {
                new SqlParameter() { ParameterName = "@IdUsuarioProduto", Value = usuarioProdutoId },
            };

            ExecuteStoredProcedure("Spc_LancarCashback_Assinatura", param, transaction);
        }
    }
}
