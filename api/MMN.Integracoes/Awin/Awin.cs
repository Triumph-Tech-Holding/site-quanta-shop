using Microsoft.Extensions.Caching.Memory;
using MMN.Util.Util;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Integracoes.Awin
{
    public class Awin
    {
        //AccessToken da BigCash
        private readonly ConcurrentDictionary<long, string> _credentials;
        private readonly RestClient _client;
        private readonly Throttler _throttler;
        private readonly IMemoryCache _cache;
        public ConcurrentBag<long> Accounts { get; private set; }

        public Awin()
        {
            _credentials = new ConcurrentDictionary<long, string>();
            _client = new RestClient("https://api.awin.com/");
            _throttler = new Throttler(15);
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = new TimeSpan(0, 15, 0)
            });
            Accounts = [];
        }

        private static RestRequest Request(string accessToken, string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            return request;
        }

        /// <summary>
        /// Provides a list of accounts you have access to 
        /// </summary>
        /// <returns></returns>
        private async Task<User> AccountsAsync(string accessToken)
        {
            try
            {
                var request = Request(accessToken, "accounts", Method.Get);
                var queryResult = await _throttler.ThrottleAsync(_client.Execute<User>, request);

                return queryResult.Data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddAccountsAsync(IEnumerable<string> accessTokens, IEnumerable<long> accountIds = null)
        {
            var tasks = accessTokens
                .Select(s => AddAccountsAsync(s, accountIds))
                .ToArray();

            foreach (var task in tasks)
            {
                await task;
            }
        }

        public async Task AddAccountsAsync(string accessToken, IEnumerable<long> accountIds = null)
        {
            var accounts = await AccountsAsync(accessToken);

            if (accounts?.accounts?.Any() != true)
            {
                return;
            }

            var selectedAccountIds = accounts.accounts
                .Where(w =>
                    accountIds == null ||
                    accountIds.Any(a => a == w.accountId))
                .Select(s => (long)s.accountId);

            foreach (var id in selectedAccountIds)
            {
                _credentials[id] = accessToken;
                Accounts.Add(id);
            }

        }

        /// <summary>
        /// Provides a list of publishers you have an active relationship with 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Programme>> ProgrammesAsync(long accountId)
        {
            try
            {
                var url = $"publishers/{accountId}/programmes?" + $"relationship=joined";
                var request = Request(_credentials[accountId], url, Method.Get);
                var queryResult = await _throttler.ThrottleAsync(_client.Execute<List<Programme>>, request);

                foreach (var item in queryResult.Data)
                    item.Detail = await ProgrammeDetailsAsync(accountId, item.id);

                return queryResult.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Provides a list of publishers you have an active relationship with 
        /// </summary>
        /// <returns></returns>
        public async Task<ProgrammeDetail> ProgrammeDetailsAsync(long accountId, int advertiserId)
        {
            try
            {
                if (!_cache.TryGetValue($"{accountId};{advertiserId}", out ProgrammeDetail programmeDetail))
                {
                    var url = $"publishers/{accountId}/programmedetails?" +
                        $"advertiserId={advertiserId}";
                    var request = Request(_credentials[accountId], url, Method.Get);
                    var queryResult = await _throttler.ThrottleAsync(_client.Execute<ProgrammeDetail>, request);

                    programmeDetail = queryResult.Data;

                    _cache.Set(
                        $"{accountId};{advertiserId}",
                        programmeDetail,
                        new TimeSpan(0, 15, 0));
                }

                return programmeDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Provides a list of your individual transactions
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Transaction>> TransactionsAsync(
            long accountId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool validationDate = false)
        {
            try
            {
                var today = DateTime.Now;

                if (startDate == null)
                    startDate = today.AddDays(-30);

                if (endDate == null)
                    endDate = today.AddDays(1);

                startDate = TimeZoneInfo.ConvertTimeToUtc(startDate.Value);
                endDate = TimeZoneInfo.ConvertTimeToUtc(endDate.Value);
                var timeZone = "UTC";
                var dateTipe = validationDate ? "validation" : "transaction";

                var url = $"publishers/{accountId}/transactions/?" +
                    $"startDate={startDate:yyyy-MM-ddThh:mm:ss}&" +
                    $"endDate={endDate:yyyy-MM-ddThh:mm:ss}&" +
                    $"timezone={timeZone}&" +
                    $"dateType={dateTipe}";
                var request = Request(_credentials[accountId], url, Method.Get);
                var response = _client.Execute(request);
                List<Transaction> transactions = null;
                // Verifique se a requisição foi bem-sucedida
                if (response.IsSuccessful)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Transaction>>(response.Content);

                    transactions = data.OrderByDescending(o => o.transactionDate).ToList();
                }

                //var queryResult = await _throttler.ThrottleAsync(_client.Execute<List<Transaction>>, request);
                //var transactions = queryResult.Data.OrderByDescending(o => o.transactionDate);

                //var programmes = await ProgrammesAsync(accountId);

                //foreach (var item in transactions)
                //{
                //    //var test = await ProgrammeDetailsAsync(accountId, item.advertiserId);
                //    var advertiser = programmes.FirstOrDefault(x => x.id == item.advertiserId);

                //    if (!string.IsNullOrEmpty(advertiser?.name))
                //    {
                //        item.advertiserName = advertiser.name;
                //    }
                //}

                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Provides individual transactions by ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Transaction> TransactionsAsync(long accountId, int id)
        {
            var url = $"publishers/{accountId}/transactions/?" +
                $"ids={id}" +
                $"&timezone=UTC";
            var request = Request(_credentials[accountId], url, Method.Get);
            var queryResult = await _throttler.ThrottleAsync(_client.Execute<List<Transaction>>, request);

            return queryResult.Data.FirstOrDefault();
        }

        /// <summary>
        /// Provides information about the commissions you get from a certain programme 
        /// </summary>
        /// <returns></returns>
        public async Task<Commission> CommissionGroupsAsync(long accountId, int advertiserId)
        {
            var url = $"publishers/{accountId}/commissiongroups?" +
                $"advertiserId={advertiserId}";
            var request = Request(_credentials[accountId], url, Method.Get);
            var queryResult = await _throttler.ThrottleAsync(_client.Execute<Commission>, request);

            return queryResult.Data;
        }

        /// <summary>
        /// Enables service partners to pull their commission sharing rules 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommisionRule>> CommissionSharingRulesAsync(long accountId)
        {
            var url = $"publishers/{accountId}/commissionsharingrules";
            var request = Request(_credentials[accountId], url, Method.Get);
            var queryResult = await _throttler.ThrottleAsync(_client.Execute<List<CommisionRule>>, request);

            return queryResult.Data;
        }

        /// <summary>
        /// Provides aggregated reports for the advertisers you work with 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReportAggregatedByAdvertiser>> ReportsAggregateByAdvertiserAsync(
            long accountId,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var today = DateTime.Now;

            if (startDate == null)
                startDate = today.AddDays(-30);

            if (endDate == null)
                endDate = today;

            startDate = TimeZoneInfo.ConvertTimeToUtc(startDate.Value);
            endDate = TimeZoneInfo.ConvertTimeToUtc(endDate.Value);

            var url = $"publishers/{accountId}/reports/advertiser?" +
                $"startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}" +
                $"&region=BR" +
                $"&timezone=America/Sao_Paulo";
            var request = Request(_credentials[accountId], url, Method.Get);
            var queryResult = await _throttler.ThrottleAsync(_client.Execute<List<ReportAggregatedByAdvertiser>>, request);

            return queryResult.Data;
        }

        /// <summary>
        /// Provides aggregated reports for the creatives you used
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReportAggregatedByCreative>> ReportsAggregatedByCreativeAsync(
            long accountId,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var today = DateTime.Now;

            if (startDate == null)
                startDate = today.AddDays(-30);

            if (endDate == null)
                endDate = today;

            startDate = TimeZoneInfo.ConvertTimeToUtc(startDate.Value);
            endDate = TimeZoneInfo.ConvertTimeToUtc(endDate.Value);

            var url = $"publishers/{accountId}/reports/creative?" +
                $"startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={startDate:yyyy-MM-dd}" +
                $"&region=BR" +
                $"&timezone=America/Sao_Paulo";
            var request = Request(_credentials[accountId], url, Method.Get);
            var queryResult = await _throttler.ThrottleAsync(_client.Execute<List<ReportAggregatedByCreative>>, request);

            return queryResult.Data;
        }
    }
}