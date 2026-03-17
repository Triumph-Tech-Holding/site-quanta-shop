using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MMN.Api.Models;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IUsersService
{
    Task<bool> SendMessageToCEO(MessageToCEO message);
    Task<IEnumerable<object>> GetBirthdays(int month);
    Task<IEnumerable<object>> GetRegistrationsPerDay(int month, int year);
    Task<IEnumerable<object>> GetRegistrationsPerDay(DateTime day);
    Task UpdateLastAccessDateAsync(Guid userId, string ipAddress, string userAgent);
    Task<int> GetDailyAccessCountAsync();
    Task<int> GetWeeklyAccessCountAsync();
    Task<int> GetMonthlyAccessCountAsync();
    Task<int> GetLastMonthAccessCountAsync();
    Task<IEnumerable<AcessoDto>> GetAccessListAsync(string login, int page, int limit);
    Task<IEnumerable<Acesso>> GetAccessesByUserIdAsync(Guid userId);
    Task<int> GetTotalAccessCountAsync(string login);
    Task<DateTime?> GetLastBuy(Guid userId);
    Task<Dictionary<Guid, DateTime?>> GetLastBuyBatch(List<Guid> userIds);
}

public class UsersService : IUsersService
{
    private readonly AppSettings _appSettings;
    private readonly IDbConnection _dbConnection;

    public UsersService(IOptions<AppSettings> appSettings, IDbConnection dbConnection)
    {
        _appSettings = appSettings.Value;
        _dbConnection = dbConnection;
    }

    public Task<bool> SendMessageToCEO(MessageToCEO message)
    {
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = message.Sender ?? _appSettings.FromName,
            DestinationName = message.Sender,
            Subject = _appSettings.FromName + " - Fale com o CEO",
            To = message.To,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ name }}", message.Sender },
                { "{{ message }}", message.Message },
                { "{{ data }}", DateTime.UtcNow.HorarioBrasilia().ToString("dd/MM/yyyy HH:mm:ss") },
                { "{{ login }}", message.Login },
                { "{{ email }}", message.From },
                { "{{ celular }}", message.Phone }
        };

        return new EmailUtilitis().EnviarEmail(body, _appSettings.FaleComCEO, null, objectEmail);
    }

    public async Task<IEnumerable<object>> GetBirthdays(int month)
    {
        const string query = @"
            SELECT 
	            UPPER(Nome) AS Nome, 
	            LOWER(Login) AS Login, 
	            LOWER(Email) AS Email, 
	            Celular,
	            FORMAT(DataNascimento, 'dd/MM') AS DataNascimento
            FROM Usuario 
            WHERE DataNascimento IS NOT NULL AND MONTH(DataNascimento) = @month
            ORDER BY MONTH(DataNascimento), DAY(DataNascimento), Nome";

        var birthdays = await _dbConnection.QueryAsync(query, new { month });

        return birthdays.ToList();
    }

    public async Task<IEnumerable<object>> GetRegistrationsPerDay(int month, int year)
    {
        const string query = @"
            DECLARE @StartDate DATE = DATEFROMPARTS(@year, @month, 1);
            DECLARE @EndDate DATE = DATEADD(HOUR, -3, GETDATE()); --EOMONTH(@StartDate);

            -- Gerar tabela de datas dentro do intervalo
            ;WITH DateRange AS (
                SELECT @StartDate AS DateValue
                UNION ALL
                SELECT DATEADD(DAY, 1, DateValue)
                FROM DateRange
                WHERE DateValue < @EndDate
            )
            SELECT DateValue
            INTO #Dates
            FROM DateRange OPTION (MAXRECURSION 0);

            -- Consulta para exibir cadastros por data
            SELECT
                ISNULL(COUNT(u.IdUsuario), 0) AS Quantidade,
                d.DateValue AS DataCadastro,
	            FORMAT(CAST(d.DateValue AS DATE), 'dd/MM/yyyy') AS DataCadastroFormatada 
            FROM
                #Dates d
            LEFT JOIN
                Usuario u ON CAST(u.DataCadastro AS DATE) = d.DateValue
            GROUP BY
                d.DateValue
            ORDER BY
                d.DateValue DESC;

            -- Remover tabela temporária
            DROP TABLE #Dates;";

        var registrations = await _dbConnection.QueryAsync(query, new { month, year });

        return registrations.ToList();
    }

    public async Task<IEnumerable<object>> GetRegistrationsPerDay(DateTime day)
    {
        const string query = @"
            SELECT 
	            UPPER(Filho.Nome) AS Nome,
	            LOWER(Filho.Login) AS Login,
	            LOWER(Filho.Email) AS Email,
	            Filho.Celular,
	            LOWER(Pai.Login) AS Patrocinador,
	            CAST(Filho.DataCadastro AS DATE) AS DataCadastro,
	            FORMAT(CAST(Filho.DataCadastro AS DATE), 'dd/MM/yyyy') AS DataCadastroFormatada,
	            Filho.EmailConfirmado,
	            Filho.IndicadoPeloQS
            FROM Usuario AS Filho
            INNER JOIN Usuario AS Pai ON Pai.IdUsuario = Filho.IdUsuarioPai
            WHERE CAST(Filho.DataCadastro AS DATE) = @day
            ORDER BY CAST(Filho.DataCadastro AS DATE) DESC";

        var registrations = await _dbConnection.QueryAsync(query, new { day });

        return registrations.ToList();
    }

    public async Task UpdateLastAccessDateAsync(Guid userId, string ipAddress, string userAgent)
    {
        var sqlUpdate = "UPDATE Usuario SET DataUltimoAcesso = @DataUltimoAcesso, EnderecoIPUltimoAcesso = @IP, AgenteUltimoAcesso = @Agente WHERE IdUsuario = @UserId;";
        var sqlInsert = @"INSERT INTO Acessos (IdUsuario, DataAcesso, Ip, Agente)
                        SELECT @UserId, @DataAcesso, @IP, @Agente
                        WHERE NOT EXISTS (
                           SELECT 1 FROM Acessos
                           WHERE IdUsuario = @UserId 
                                 AND CAST(DataAcesso AS DATE) = CAST(@DataAcesso AS DATE) 
                                 AND Agente = @Agente
                        );";

        var dataUltimoAcesso = DateTime.Now;

        var parameters = new
        {
            DataUltimoAcesso = dataUltimoAcesso,
            UserId = userId,
            DataAcesso = dataUltimoAcesso,
            IP = ipAddress,
            Agente = userAgent

        };

        await _dbConnection.ExecuteAsync(sqlUpdate, new { DataUltimoAcesso = dataUltimoAcesso, UserId = userId, IP = ipAddress, Agente = userAgent });
        await _dbConnection.ExecuteAsync(sqlInsert, parameters);
    }

    public async Task<int> GetDailyAccessCountAsync()
    {
        var query = @"SELECT COUNT(*) FROM Acessos WHERE CAST(DataAcesso AS DATE) = CAST(DATEADD(HOUR, -3, GETDATE()) AS DATE)";
        return await _dbConnection.ExecuteScalarAsync<int>(query);
    }

    public async Task<int> GetWeeklyAccessCountAsync()
    {
        var query = @"SELECT COUNT(*) FROM Acessos WHERE CAST(DataAcesso AS DATE) BETWEEN CAST(DATEADD(DAY, -6, DATEADD(HOUR, -3, GETDATE())) AS DATE) AND CAST(DATEADD(HOUR, -3, GETDATE()) AS DATE)";
        return await _dbConnection.ExecuteScalarAsync<int>(query);
    }
    
    public async Task<int> GetMonthlyAccessCountAsync()
    {
        var query = @"SELECT COUNT(*) FROM Acessos WHERE MONTH(DataAcesso) = MONTH(DATEADD(HOUR, -3, GETDATE())) AND YEAR(DataAcesso) = YEAR(DATEADD(HOUR, -3, GETDATE()))";
        return await _dbConnection.ExecuteScalarAsync<int>(query);
    }

    public async Task<int> GetLastMonthAccessCountAsync()
    {
        var query = @"SELECT COUNT(*) FROM Acessos WHERE MONTH(DataAcesso) = MONTH(DATEADD(MONTH, -1, DATEADD(HOUR, -3, GETDATE()))) AND YEAR(DataAcesso) = YEAR(DATEADD(MONTH, -1, DATEADD(HOUR, -3, GETDATE())))";
        return await _dbConnection.ExecuteScalarAsync<int>(query);
    }

    public async Task<IEnumerable<AcessoDto>> GetAccessListAsync(string login, int page, int limit)
    {
        var query = @"WITH DistinctAcessos AS (
	                    SELECT 
		                    a.IdUsuario, 
		                    u.Nome AS UsuarioNome, 
		                    u.Login, 
		                    u.Email,
		                    u.DataUltimoAcesso,
		                    ROW_NUMBER() OVER (PARTITION BY a.IdUsuario ORDER BY a.DataAcesso DESC) AS rn
	                    FROM Acessos a
	                    JOIN Usuario u ON a.IdUsuario = u.IdUsuario
                    )
                    SELECT 
	                    IdUsuario, 
	                    UsuarioNome, 
	                    Login, 
	                    Email, 
	                    DataUltimoAcesso,
	                    (SELECT MAX(DataPedido) 
                         FROM Pedido 
                         WHERE IdUsuario = da.IdUsuario AND (IdAwinTransaction IS NOT NULL OR IdUsuarioComerciante IS NOT NULL)) AS DataUltimaCompra
                    FROM DistinctAcessos AS da
                    WHERE rn = 1 AND (@Login IS NULL OR Login LIKE '%' + @Login + '%')
                    ORDER BY UsuarioNome
                    OFFSET @Offset ROWS
                    FETCH NEXT @limit ROWS ONLY;";

        return await _dbConnection.QueryAsync<AcessoDto>(query, new { Offset = (page - 1) * limit, Limit = limit, Login = login });
    }

    public async Task<IEnumerable<Acesso>> GetAccessesByUserIdAsync(Guid userId)
    {
        var query = @"
        SELECT * FROM Acessos
        WHERE IdUsuario = @UserId AND MONTH(DataAcesso) = MONTH(DATEADD(HOUR, -3, GETDATE())) AND YEAR(DataAcesso) = YEAR(DATEADD(HOUR, -3, GETDATE()))
        ORDER BY DataAcesso DESC";

        return await _dbConnection.QueryAsync<Acesso>(query, new { UserId = userId });
    }

    public async Task<int> GetTotalAccessCountAsync(string login)
    {
        var sql = "SELECT COUNT(DISTINCT A.IdUsuario) FROM Acessos AS A INNER JOIN Usuario AS U ON U.IdUsuario = A.IdUsuario ";

        if (!string.IsNullOrEmpty(login))
        {
            sql += " WHERE Login LIKE @Login"; // Ajuste conforme sua estrutura de dados
        }

        return await _dbConnection.ExecuteScalarAsync<int>(sql, new { Login = $"%{login}%" });
    }

    public async Task<DateTime?> GetLastBuy(Guid userId)
    {
        var query = @"SELECT MAX(DataPedido) FROM Pedido WHERE IdUsuario = @UserId AND (IdAwinTransaction IS NOT NULL OR IdUsuarioComerciante IS NOT NULL)";
        return await _dbConnection.ExecuteScalarAsync<DateTime?>(query, new { UserId = userId });
    }

    public async Task<Dictionary<Guid, DateTime?>> GetLastBuyBatch(List<Guid> userIds)
    {
        if (userIds == null || !userIds.Any())
            return new Dictionary<Guid, DateTime?>();

        var query = @"
            SELECT 
                IdUsuario,
                MAX(DataPedido) AS DataUltimaCompra
            FROM Pedido 
            WHERE IdUsuario IN @UserIds 
              AND (IdAwinTransaction IS NOT NULL OR IdUsuarioComerciante IS NOT NULL)
            GROUP BY IdUsuario";

        var results = await _dbConnection.QueryAsync<(Guid IdUsuario, DateTime? DataUltimaCompra)>(
            query, 
            new { UserIds = userIds });

        return userIds.ToDictionary(
            id => id,
            id => results.FirstOrDefault(r => r.IdUsuario == id).DataUltimaCompra
        );
    }
}
