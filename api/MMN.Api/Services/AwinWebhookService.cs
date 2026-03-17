using Dapper;
using MMN.Api.Controllers.v2;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IAwinWebhookService
{
    Task<AwinWebhookRequest> GetByIdAsync(string transactionId);
    Task<IEnumerable<AwinWebhookRequest>> GetAllAsync();
    Task CreateAsync(AwinWebhookRequest request);
    Task UpdateAsync(AwinWebhookRequest request);
    Task DeleteAsync(string transactionId);
}

public class AwinWebhookService : IAwinWebhookService
{
    private readonly IDbConnection _dbConnection;

    public AwinWebhookService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<AwinWebhookRequest> GetByIdAsync(string transactionId)
    {
        const string sql = "SELECT * FROM AwinWebhookRequests WHERE TransactionId = @TransactionId";
        return await _dbConnection.QuerySingleOrDefaultAsync<AwinWebhookRequest>(sql, new { TransactionId = transactionId });
    }

    public async Task<IEnumerable<AwinWebhookRequest>> GetAllAsync()
    {
        const string sql = "SELECT * FROM AwinWebhookRequests";
        return await _dbConnection.QueryAsync<AwinWebhookRequest>(sql);
    }

    public async Task CreateAsync(AwinWebhookRequest request)
    {
        const string sql = @"
            INSERT INTO AwinWebhookRequests (TransactionId, TransactionDate, TransactionCurrency, TransactionAmount, 
                                              AffiliateId, MerchantId, BannerId, ClickRef, ClickThroughTime, Commission)
            VALUES (@TransactionId, @TransactionDate, @TransactionCurrency, @TransactionAmount, 
                    @AffiliateId, @MerchantId, @BannerId, @ClickRef, @ClickThroughTime, @Commission)";
        await _dbConnection.ExecuteAsync(sql, request);
    }

    public async Task UpdateAsync(AwinWebhookRequest request)
    {
        const string sql = @"
            UPDATE AwinWebhookRequests
            SET TransactionDate = @TransactionDate,
                TransactionCurrency = @TransactionCurrency,
                TransactionAmount = @TransactionAmount,
                AffiliateId = @AffiliateId,
                MerchantId = @MerchantId,
                BannerId = @BannerId,
                ClickRef = @ClickRef,
                ClickThroughTime = @ClickThroughTime,
                Commission = @Commission
            WHERE TransactionId = @TransactionId";
        await _dbConnection.ExecuteAsync(sql, request);
    }

    public async Task DeleteAsync(string transactionId)
    {
        const string sql = "DELETE FROM AwinWebhookRequests WHERE TransactionId = @TransactionId";
        await _dbConnection.ExecuteAsync(sql, new { TransactionId = transactionId });
    }
}