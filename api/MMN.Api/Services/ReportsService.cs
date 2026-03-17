using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IReportsService
{
    Task<SalesWaitingForApprovalResponse> GetSalesWaitingForApproval(string seller, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
    Task<InvoicesAwaitingPaymentResponse> GetInvoicesAwaitingPayment(string seller, DateTime? startDate, DateTime? endDate);
    Task<InstallmentsAwaitingPaymentResponse> GetInstallmentsAwaitingPayment(string login, string name, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
    Task<List<Installments>> GetInstallmentsByOrder(int orderId);
}

public class ReportsService : IReportsService
{
    private readonly IDbConnection _dbConnection;

    public ReportsService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<SalesWaitingForApprovalResponse> GetSalesWaitingForApproval(string seller, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
    {
        string queryTotalizers = @"
                    DECLARE @Vendedor VARCHAR(500) = @VendedorParam;
                    DECLARE @DataDe DATE = ISNULL(@DataDeParam, DATEFROMPARTS(YEAR(DATEADD(YEAR, -10, GETDATE())), 1, 1));
                    DECLARE @DataAte DATE = ISNULL(@DataAteParam, EOMONTH(GETDATE()));

                    SELECT
                        SUM(CC.Valor) AS TotalEmVendas,
	                    SUM(CC.Valor * CC.PercentualCashback) AS TotalEmCashback,
	                    AVG(CC.PercentualCashback) AS MediaCashback
                    FROM CuponCashback AS CC
                    LEFT OUTER JOIN CuponCashbackPedido AS CCP ON CCP.IdCuponCashback = CC.IdCuponCashback
                    INNER JOIN Usuario AS Consumidor ON Consumidor.Documento = CC.Documento
                    INNER JOIN Usuario AS Vendedor ON Vendedor.IdUsuario = CC.IdComerciante
                    INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CC.IdComerciante
                    INNER JOIN Usuario AS Credenciador ON Credenciador.IdUsuario = Credenciamento.IdUsuarioPai
                    WHERE 
                        1 = 1
                        AND CCP.IdPedido IS NULL
                        AND CC.Status IN (0, 1)
                        AND (@Vendedor IS NULL OR Credenciamento.Estabelecimento LIKE '%' + @Vendedor + '%')
                        AND CC.DataCompra BETWEEN @DataDe AND @DataAte";

        string queryCount = @"
                    DECLARE @Vendedor VARCHAR(500) = @VendedorParam;
                    DECLARE @DataDe DATE = ISNULL(@DataDeParam, DATEFROMPARTS(YEAR(DATEADD(YEAR, -10, GETDATE())), 1, 1));
                    DECLARE @DataAte DATE = ISNULL(@DataAteParam, EOMONTH(GETDATE()));
                    DECLARE @PageNumber INT = @PageNumberParam;
                    DECLARE @PageSize INT = @PageSizeParam;
                    
                    SELECT 
                        COUNT(*) AS TotalItems
                    FROM CuponCashback AS CC
                    LEFT OUTER JOIN CuponCashbackPedido AS CCP ON CCP.IdCuponCashback = CC.IdCuponCashback
                    INNER JOIN Usuario AS Consumidor ON Consumidor.Documento = CC.Documento
                    INNER JOIN Usuario AS Vendedor ON Vendedor.IdUsuario = CC.IdComerciante
                    INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CC.IdComerciante
                    INNER JOIN Usuario AS Credenciador ON Credenciador.IdUsuario = Credenciamento.IdUsuarioPai
                    WHERE 
                        1 = 1
                        AND CCP.IdPedido IS NULL
                        AND CC.Status IN (0, 1)
                        AND (@Vendedor IS NULL OR Credenciamento.Estabelecimento LIKE '%' + @Vendedor + '%')
                        AND CC.DataCompra BETWEEN @DataDe AND @DataAte;";

        string query = @"
                    DECLARE @Vendedor VARCHAR(500) = @VendedorParam;
                    DECLARE @DataDe DATE = ISNULL(@DataDeParam, DATEFROMPARTS(YEAR(DATEADD(YEAR, -10, GETDATE())), 1, 1));
                    DECLARE @DataAte DATE = ISNULL(@DataAteParam, EOMONTH(GETDATE()));
                    DECLARE @PageNumber INT = @PageNumberParam;
                    DECLARE @PageSize INT = @PageSizeParam;

                    WITH SalesWaitingForApproval AS (
                        SELECT
                            ROW_NUMBER() OVER (ORDER BY Vendedor.Login, CC.DataCompra) AS RowNum,
                            Consumidor.Login AS Consumidor, 
                            Credenciamento.Estabelecimento AS Vendedor,
                            Credenciador.Login AS Credenciador,
                            CC.IdCuponCashback,
                            CC.Token,
                            CC.Valor,
                            CC.PercentualCashback,
                            CC.DataCompra,
                            CC.Descricao,
                            CC.ComprovanteCompra
                        FROM CuponCashback AS CC
                        LEFT OUTER JOIN CuponCashbackPedido AS CCP ON CCP.IdCuponCashback = CC.IdCuponCashback
                        INNER JOIN Usuario AS Consumidor ON Consumidor.Documento = CC.Documento
                        INNER JOIN Usuario AS Vendedor ON Vendedor.IdUsuario = CC.IdComerciante
                        INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CC.IdComerciante
                        INNER JOIN Usuario AS Credenciador ON Credenciador.IdUsuario = Credenciamento.IdUsuarioPai
                        WHERE 
                            1 = 1
                            AND CCP.IdPedido IS NULL
                            AND CC.Status IN (0, 1)
                            AND (@Vendedor IS NULL OR Credenciamento.Estabelecimento LIKE '%' + @Vendedor + '%')
                            AND CC.DataCompra BETWEEN @DataDe AND @DataAte
                    )
                    SELECT *
                    FROM SalesWaitingForApproval
                    WHERE RowNum BETWEEN (@PageNumber - 1) * @PageSize + 1 AND @PageNumber * @PageSize;";

        var parameters = new
        {
            VendedorParam = seller,
            DataDeParam = startDate,
            DataAteParam = endDate,
            PageNumberParam = pageNumber,
            PageSizeParam = pageSize
        };

        var items = await _dbConnection.QueryAsync<SalesWaitingForApproval>(query, parameters);
        var totalItems = await _dbConnection.ExecuteScalarAsync<int>(queryCount, parameters);
        var totalizers = await _dbConnection.QuerySingleOrDefaultAsync<SalesWaitingForApprovalTotalizers>(queryTotalizers, parameters);

        return new SalesWaitingForApprovalResponse
        {
            Items = items.ToList(),
            TotalItems = totalItems,
            Totalizers = totalizers
        };
    }

    public async Task<InvoicesAwaitingPaymentResponse> GetInvoicesAwaitingPayment(string seller, DateTime? startDate, DateTime? endDate)
    {
        string queryTotalizers = @"
                        DECLARE @Vendedor VARCHAR(500) = @VendedorParam;
                        DECLARE @DataDe DATE = ISNULL(@DataDeParam, DATEFROMPARTS(YEAR(DATEADD(YEAR, -10, GETDATE())), 1, 1));
                        DECLARE @DataAte DATE = ISNULL(@DataAteParam, EOMONTH(GETDATE()));

                        SELECT
	                        SUM(P.ValorPedido - P.ValorTaxa) AS TotalValue,
	                        COUNT(*) AS TotalInvoicesAwaitingPayment
                        FROM Pedido AS P
                        INNER JOIN Pagamento AS PG ON PG.IdPedido = P.IdPedido
                        INNER JOIN Usuario AS U ON U.IdUsuario = P.IdUsuario
                        INNER JOIN Credenciamento ON Credenciamento.IdUsuario = U.IdUsuario
                        WHERE 
                            1 = 1
                            AND (@Vendedor IS NULL OR Credenciamento.Estabelecimento LIKE '%' + @Vendedor + '%')
                            AND PG.DataValidade BETWEEN @DataDe AND @DataAte 
	                        AND P.Ativo = 1
	                        AND P.Pago = 0
	                        AND P.Tipo = 5
	                        AND P.DataPagamento IS NULL
	                        AND PG.DataPagamento IS NULL";

        string query = @"
                        DECLARE @Vendedor VARCHAR(500) = @VendedorParam;
                        DECLARE @DataDe DATE = ISNULL(@DataDeParam, DATEFROMPARTS(YEAR(DATEADD(YEAR, -10, GETDATE())), 1, 1));
                        DECLARE @DataAte DATE = ISNULL(@DataAteParam, EOMONTH(GETDATE()));

                        WITH FaturasEmAtraso AS (
                        SELECT
	                        UPPER(Credenciamento.Estabelecimento) AS Estabelecimento,
	                        P.IdPedido,
                            P.ValorPedido - P.ValorTaxa AS Valor,
	                        P.ValorTaxa,
	                        P.ValorPedido AS ValorTotal,
	                        P.DataPedido,
                            P.MeioPagamento,
	                        PG.DataValidade,
	                        PG.UrlBoleto,
	                        PG.LinhaDigitavelBoleto,
	                        PG.CodigoReferenciaBoleto,
	                        IIF(
		                        DATEDIFF(DAY, PG.DataValidade, GETDATE()) < 0, 
		                        'Aguardando pagamento', 
		                        CAST(DATEDIFF(DAY, PG.DataValidade, GETDATE()) AS VARCHAR(100))) AS DiasAtraso
                        FROM Pedido AS P
                        INNER JOIN Pagamento AS PG ON PG.IdPedido = P.IdPedido
                        INNER JOIN Usuario AS U ON U.IdUsuario = P.IdUsuario
                        INNER JOIN Credenciamento ON Credenciamento.IdUsuario = U.IdUsuario
                        WHERE 
                            1 = 1
                            AND (@Vendedor IS NULL OR Credenciamento.Estabelecimento LIKE '%' + @Vendedor + '%')
                            AND PG.DataValidade BETWEEN @DataDe AND @DataAte 
	                        AND P.Ativo = 1
	                        AND P.Pago = 0
	                        AND P.Tipo = 5
	                        AND P.DataPagamento IS NULL
	                        AND PG.DataPagamento IS NULL
                        )

                        SELECT * FROM FaturasEmAtraso
                        --WHERE DiasAtraso <> 'Aguardando pagamento'
                        ORDER BY DataPedido DESC

                        -- Tipo 5 - Fatura de cashback
                        -- Tipo 1 - Parcelamento de habilitação";

        var parameters = new
        {
            VendedorParam = seller,
            DataDeParam = startDate,
            DataAteParam = endDate
        };

        var items = await _dbConnection.QueryAsync<InvoicesAwaitingPayment>(query, parameters);
        var totalizers = await _dbConnection.QuerySingleOrDefaultAsync<InvoicesAwaitingPaymentTotalizers>(queryTotalizers, parameters);

        return new InvoicesAwaitingPaymentResponse
        {
            Items = items.ToList(),
            Totalizers = totalizers
        };
    }

    public async Task<InstallmentsAwaitingPaymentResponse> GetInstallmentsAwaitingPayment(string login, string name, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
    {
        string queryAwaitingTotalizers = @"
                        DECLARE @LoginPesquisa NVARCHAR(50) = @Login;  -- Parâmetro para pesquisa de Login
                        DECLARE @NomePesquisa NVARCHAR(50) = @Name;  -- Parâmetro para pesquisa de Nome
                        DECLARE @DataInicial DATE = ISNULL(@StartDate, CAST(DATEFROMPARTS(YEAR(GETDATE()) - 10, 1, 1) AS DATE));
                        DECLARE @DataFinal DATE = ISNULL(@EndDate, EOMONTH(GETDATE()));

                        SELECT 
	                        SUM(Pagamento.Valor) AS TotalValue, 
	                        COUNT(*) AS TotalInstallmentsAwaitingPayment 
                        FROM Pagamento
                        LEFT OUTER JOIN Pedido ON Pedido.IdPedido = Pagamento.IdPedido
                        LEFT OUTER JOIN Usuario ON Usuario.IdUsuario = Pedido.IdUsuario
                        WHERE 
	                        1 = 1
	                        AND Pagamento.Pago = 0
	                        AND Pedido.Tipo = 1
                          AND Pedido.Pago = 0
                          AND Pedido.Ativo = 1
                          AND Pedido.Cancelado = 0
	                      AND (LOWER(REPLACE(Usuario.Login, ' ', '')) LIKE '%' + LOWER(REPLACE(@LoginPesquisa, ' ', '')) + '%' OR @LoginPesquisa IS NULL)
                          AND (UPPER(REPLACE(Usuario.Nome, ' ', '')) LIKE '%' + UPPER(REPLACE(@NomePesquisa, ' ', '')) + '%' OR @NomePesquisa IS NULL)
	                      AND Pedido.DataPedido BETWEEN @DataInicial AND @DataFinal;";

        string queryPaidTotalizers = @"
                        DECLARE @LoginPesquisa NVARCHAR(50) = @Login;  -- Parâmetro para pesquisa de Login
                        DECLARE @NomePesquisa NVARCHAR(50) = @Name;  -- Parâmetro para pesquisa de Nome
                        DECLARE @DataInicial DATE = ISNULL(@StartDate, CAST(DATEFROMPARTS(YEAR(GETDATE()) - 10, 1, 1) AS DATE));
                        DECLARE @DataFinal DATE = ISNULL(@EndDate, EOMONTH(GETDATE()));


                        SELECT
	                        SUM(Pagamento.Valor) AS TotalValue, 
	                        COUNT(*) AS TotalPaidInstallments
                        FROM Pagamento
                        LEFT OUTER JOIN Pedido ON Pedido.IdPedido = Pagamento.IdPedido
                        LEFT OUTER JOIN Usuario ON Usuario.IdUsuario = Pedido.IdUsuario
                        WHERE 
	                        1 = 1
	                        AND Pagamento.Pago = 1
	                        AND Pedido.Tipo = 1
                          AND Pedido.Pago = 0
                          AND Pedido.Ativo = 1
                          AND Pedido.Cancelado = 0
	                        AND (LOWER(REPLACE(Usuario.Login, ' ', '')) LIKE '%' + LOWER(REPLACE(@LoginPesquisa, ' ', '')) + '%' OR @LoginPesquisa IS NULL)
                          AND (UPPER(REPLACE(Usuario.Nome, ' ', '')) LIKE '%' + UPPER(REPLACE(@NomePesquisa, ' ', '')) + '%' OR @NomePesquisa IS NULL)
	                        AND Pedido.DataPedido BETWEEN @DataInicial AND @DataFinal;";

        string queryCount = @"
                        DECLARE @LoginPesquisa NVARCHAR(50) = @Login;
                        DECLARE @NomePesquisa NVARCHAR(50) = @Name;
                        DECLARE @DataInicial DATE = ISNULL(@StartDate, CAST(DATEFROMPARTS(YEAR(GETDATE()) - 10, 1, 1) AS DATE));
                        DECLARE @DataFinal DATE = ISNULL(@EndDate, EOMONTH(GETDATE()));

                        SELECT 
                           COUNT(*) AS TotalItems
                        FROM Pedido
                        LEFT OUTER JOIN Usuario ON Usuario.IdUsuario = Pedido.IdUsuario
                        LEFT OUTER JOIN Produto ON Produto.Valor = Pedido.ValorPedido
                        WHERE 
                            Pedido.Tipo = 1
                            AND Pedido.Pago = 0
                            AND Pedido.Ativo = 1
                            AND Pedido.Cancelado = 0
		                        AND (LOWER(REPLACE(Usuario.Login, ' ', '')) LIKE '%' + LOWER(REPLACE(@LoginPesquisa, ' ', '')) + '%' OR @LoginPesquisa IS NULL)
                            AND (UPPER(REPLACE(Usuario.Nome, ' ', '')) LIKE '%' + UPPER(REPLACE(@NomePesquisa, ' ', '')) + '%' OR @NomePesquisa IS NULL)
		                        AND Pedido.DataPedido BETWEEN @DataInicial AND @DataFinal;";

        string query = @"
                        DECLARE @LoginPesquisa NVARCHAR(50) = @Login;  -- Parâmetro para pesquisa de Login
                        DECLARE @NomePesquisa NVARCHAR(50) = @Name;  -- Parâmetro para pesquisa de Nome
                        DECLARE @DataInicial DATE = ISNULL(@StartDate, CAST(DATEFROMPARTS(YEAR(GETDATE()) - 10, 1, 1) AS DATE));
                        DECLARE @DataFinal DATE = ISNULL(@EndDate, EOMONTH(GETDATE()));
                        --DECLARE @Page INT = 0;  -- Página inicial, altere conforme necessário
                        --DECLARE @PerPage INT = 50;  -- Número de registros por página

                        SELECT 
                            UPPER(Usuario.Nome) AS Nome,
                            LOWER(Usuario.Login) AS Login,
                            Usuario.Documento,
                            Usuario.Celular,
                            Produto.Nome AS Habilitacao,
                            Pedido.IdPedido,
		                    Pedido.ValorPedido,
		                    Pedido.DataPedido,
		                    Pedido.NumeroParcelas
                        FROM Pedido
                        LEFT OUTER JOIN Usuario ON Usuario.IdUsuario = Pedido.IdUsuario
                        LEFT OUTER JOIN Produto ON Produto.Valor = Pedido.ValorPedido
                        WHERE 
                            Pedido.Tipo = 1
                            AND Pedido.Pago = 0
                            AND Pedido.Ativo = 1
                            AND Pedido.Cancelado = 0
		                        AND (LOWER(REPLACE(Usuario.Login, ' ', '')) LIKE '%' + LOWER(REPLACE(@LoginPesquisa, ' ', '')) + '%' OR @LoginPesquisa IS NULL)
                            AND (UPPER(REPLACE(Usuario.Nome, ' ', '')) LIKE '%' + UPPER(REPLACE(@NomePesquisa, ' ', '')) + '%' OR @NomePesquisa IS NULL)
		                        AND Pedido.DataPedido BETWEEN @DataInicial AND @DataFinal
                        ORDER BY
                            Usuario.Nome, Pedido.DataPedido
                        OFFSET @Page * @PerPage ROWS 
                        FETCH NEXT @PerPage ROWS ONLY;";

        var parameters = new
        {
            Login = login,
            Name = name,
            StartDate = startDate,
            EndDate = endDate,
            Page = pageNumber - 1,
            PerPage = pageSize
        };

        var items = await _dbConnection.QueryAsync<InstallmentsAwaitingPayment>(query, parameters);
        var totalItems = await _dbConnection.ExecuteScalarAsync<int>(queryCount, parameters);
        var totalizersAwaiting = await _dbConnection.QuerySingleOrDefaultAsync<InstallmentsAwaitingPaymentTotalizers>(queryAwaitingTotalizers, parameters);
        var totalizersPaid = await _dbConnection.QuerySingleOrDefaultAsync<PaidInstallmentsTotalizers>(queryPaidTotalizers, parameters);

        return new InstallmentsAwaitingPaymentResponse
        {
            Items = items.ToList(),
            TotalItems = totalItems,
            TotalizersAwaiting = totalizersAwaiting,
            TotalizersPaid = totalizersPaid
        };
    }

    public async Task<List<Installments>> GetInstallmentsByOrder(int orderId)
    {
        string query = @"SELECT * FROM Pagamento WHERE IdPedido = @OrderId ORDER BY NumeroParcela";

        var parameters = new
        {
            OrderId = orderId
        };

        var items = await _dbConnection.QueryAsync<Installments>(query, parameters);

        return items.ToList();         
    }
}

public class SalesWaitingForApproval
{
    public string Consumidor { get; set; }
    public string Vendedor { get; set; }
    public string Credenciador { get; set; }
    public string IdCuponCashback { get; set; }
    public string Token { get; set; }
    public decimal Valor { get; set; }
    public decimal PercentualCashback { get; set; }
    public DateTime DataCompra { get; set; }
    public string Descricao { get; set; }
    public string ComprovanteCompra { get; set; }
    public int TotalItems { get; set; }
}

public class SalesWaitingForApprovalTotalizers
{
    public decimal TotalEmVendas { get; set; }
    public decimal TotalEmCashback { get; set; }
    public decimal MediaCashback { get; set; }
}

public class SalesWaitingForApprovalResponse
{
    public IEnumerable<SalesWaitingForApproval> Items { get; set; }
    public int TotalItems { get; set; }
    public SalesWaitingForApprovalTotalizers Totalizers { get; set; }
}

public class InvoicesAwaitingPayment
{
    public long IdPedido { get; set; }
    public string Estabelecimento { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorTaxa { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataPedido { get; set; }
    public DateTime DataValidade { get; set; }
    public int MeioPagamento { get; set; }
    public string UrlBoleto { get; set; }
    public string LinhaDigitavelBoleto { get; set; }
    public string CodigoReferenciaBoleto { get; set; }
    public string DiasAtraso { get; set; }
}

public class InvoicesAwaitingPaymentTotalizers
{
    public decimal TotalValue { get; set; }
    public int TotalInvoicesAwaitingPayment { get; set; }
}

public class InvoicesAwaitingPaymentResponse
{
    public IEnumerable<InvoicesAwaitingPayment> Items { get; set; }
    public InvoicesAwaitingPaymentTotalizers Totalizers { get; set; }
}

public class InstallmentsAwaitingPayment
{
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Documento { get; set; }
    public string Celular       { get; set; }
    public string Habilitacao   { get; set; }
    public int IdPedido        { get; set; }
    public decimal ValorPedido  { get; set; }
    public DateTime DataPedido { get; set; }
    public int NumeroParcelas { get; set; }
}

public class InstallmentsAwaitingPaymentTotalizers
{
    public decimal TotalValue { get; set; }
    public int TotalInstallmentsAwaitingPayment { get; set; }
}

public class PaidInstallmentsTotalizers
{
    public decimal TotalValue { get; set; }
    public int TotalPaidInstallments { get; set; }
}

public class InstallmentsAwaitingPaymentResponse
{
    public IEnumerable<InstallmentsAwaitingPayment> Items { get; set; }
    public int TotalItems { get; set; }
    public InstallmentsAwaitingPaymentTotalizers TotalizersAwaiting { get; set; }
    public PaidInstallmentsTotalizers TotalizersPaid { get; set; }
}

public class Installments
{ 
    public int IdPagamento { get; set; }
    public int IdPedido { get; set; }
    public bool Pago { get; set; }
    public decimal Valor { get; set; }
    public int NumeroParcela { get; set; }
    public DateTime? DataValidade { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string CodigoReferenciaBoleto { get; set; }
    public string LinhaDigitavelBoleto { get; set; }
    public string UrlBoleto { get; set; }
    public bool Ativo { get; set; }
    public DateTime? DataReferencia { get; set; }
    public int Status { get; set; }
}