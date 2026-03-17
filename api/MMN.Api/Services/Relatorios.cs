using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Repositorio.Contexto;
using System;
using Microsoft.Data.SqlClient;
using Dapper;

namespace MMN.Api.Service
{
    public interface IRelatorios
    {
        bool IsDev { get; set; }

        object RelatorioVendasAwin();
    }

    public class Relatorios : IRelatorios
    {
        private readonly IServiceProvider _serviceProvider;
        public bool IsDev { get; set; }

        private IServiceScope _scope;

        public Relatorios(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _serviceProvider = serviceProvider;
            IsDev = env.IsDevelopment();
            _scope = _serviceProvider.CreateScope();
        }

        public object RelatorioVendasAwin()
        {
            try
            {
                var query = @"SELECT 
                                    P.IdPedido,
                                    IdAwinTransaction,
                                    U.IdUsuario,
                                    LOWER(U.Email) AS Email,
                                    UPPER(U.Nome) AS Nome,
                                    DataPedido,
                                    ValorPedido,
                                    Cashback,
                                    Pago,
                                    DataPagamento,
                                    ValorPago
                            FROM Pedido AS P
                            INNER JOIN Usuario AS U ON U.IdUsuario = P.IdUsuario
                            WHERE P.IdAwinTransaction IS NOT NULL AND P.Tipo = 3
                            ORDER BY P.DataPedido DESC";
                var context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var connectionString = context.Database.GetDbConnection().ConnectionString;

                using var connection = new SqlConnection(connectionString);

                return connection.Query<RelatorioVendasAwin>(query);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    class RelatorioVendasAwin
    {
        public long IdPedido { get; set; }
        public long IdAwinTransaction { get; set; }
        public Guid IdUsuario { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime? DataPedido { get; set; }
        public decimal? ValorPedido { get; set; }
        public decimal? Cashback { get; set; }
        public bool Pago { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorPago { get; set; }
    }
}
