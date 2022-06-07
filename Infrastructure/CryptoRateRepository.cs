using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;


namespace Infrastructure
{
    public class CryptoRateRepository : ICryptoRateRepository
    {
        private readonly IHttpClientFactory _factory;
        private string _baseUrl = "https://api.coingecko.com/api/v3/simple/price";
        private ICurrencyRateRepository _currencyRateRepository;

        public CryptoRateRepository(IHttpClientFactory factory, ICurrencyRateRepository currencyRateRepository)
        {
            _factory = factory;
            _currencyRateRepository = currencyRateRepository;
        }

        public async Task<decimal> GetCurrentPriceInCurrency(string cryptoId, string currencyCode)
        {
            var client = _factory.CreateClient();

            var query = new Dictionary<string, string>()
            {
                ["ids"] = cryptoId,
                ["vs_currencies"] = currencyCode,
                ["include_24hr_change"] = "true"
            };
            var uri = QueryHelpers.AddQueryString(_baseUrl, query);
            var apiResult = await client.GetAsync(uri);
            if (!apiResult.IsSuccessStatusCode)
                throw new ApplicationException("Cannot call the coingeckopi for crypto price");

            var apiResultJson = await apiResult.Content.ReadAsStringAsync();

            var tokensFromResult = apiResultJson.Split(':', ',', '{', '}');
            var price = decimal.Parse(tokensFromResult[4]);
            return price;
        }

        public async Task<decimal> GetPastPriceInCurrency(string cryptoId, string currencyCode, DateTime dateTime)
        {
            var client = _factory.CreateClient();
            string strDateTime = dateTime.ToString("dd-MM-yyyy");
            var baseUrl
                = $"https://api.coingecko.com/api/v3/coins/{cryptoId}/history?date={strDateTime}&localization=false";
            client.BaseAddress = new Uri(baseUrl);

            var apiResult = await client.GetAsync("/");
            if (!apiResult.IsSuccessStatusCode)
                throw new ApplicationException("Error while calling the pass crypto API");
            var apiResultString = await apiResult.Content.ReadAsStringAsync();
            var passPriceDto = JsonSerializer.Deserialize<PassPriceDto>(apiResultString, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            if (passPriceDto is null)
                throw new ApplicationException("Error while calling the pass crypto API");
            var priceInUsd = passPriceDto.GetPriceValueInUsd();
            var rateObj = await _currencyRateRepository.GetRateObject("USD");
            return priceInUsd * rateObj.GetValue(currencyCode);

        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetListCoinPrice(string coinIds, string currencies)
        {
            var client = _factory.CreateClient();

            var query = new Dictionary<string, string>()
            {
                ["ids"] = coinIds,
                ["vs_currencies"] = currencies,
            };
            var uri = QueryHelpers.AddQueryString(_baseUrl, query);
            var apiResult = await client.GetAsync(uri);
            if (!apiResult.IsSuccessStatusCode)
                throw new ApplicationException("Cannot call the coingecko api for crypto price");
            var apiResultString = await apiResult.Content.ReadAsStringAsync();
            Console.WriteLine(apiResultString);
            var priceObject = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(apiResultString);

            return priceObject;
        }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CodeAdditionsDeletions4Weeks
    {
        public object additions { get; set; }
        public object deletions { get; set; }
    }

    public class CommunityData
    {
        public object facebook_likes { get; set; }
        public int twitter_followers { get; set; }
        public double reddit_average_posts_48h { get; set; }
        public double reddit_average_comments_48h { get; set; }
        public int reddit_subscribers { get; set; }
        public string reddit_accounts_active_48h { get; set; }
    }

    public class CurrentPrice
    {
        public double aed { get; set; }
        public double ars { get; set; }
        public double aud { get; set; }
        public double bch { get; set; }
        public double bdt { get; set; }
        public double bhd { get; set; }
        public double bmd { get; set; }
        public double brl { get; set; }
        public int btc { get; set; }
        public double cad { get; set; }
        public double chf { get; set; }
        public double clp { get; set; }
        public double cny { get; set; }
        public double czk { get; set; }
        public double dkk { get; set; }
        public double eth { get; set; }
        public double eur { get; set; }
        public double gbp { get; set; }
        public double hkd { get; set; }
        public double huf { get; set; }
        public double idr { get; set; }
        public double ils { get; set; }
        public double inr { get; set; }
        public double jpy { get; set; }
        public double krw { get; set; }
        public double kwd { get; set; }
        public double lkr { get; set; }
        public double ltc { get; set; }
        public double mmk { get; set; }
        public double mxn { get; set; }
        public double myr { get; set; }
        public double ngn { get; set; }
        public double nok { get; set; }
        public double nzd { get; set; }
        public double php { get; set; }
        public double pkr { get; set; }
        public double pln { get; set; }
        public double rub { get; set; }
        public double sar { get; set; }
        public double sek { get; set; }
        public double sgd { get; set; }
        public double thb { get; set; }
        public double @try { get; set; }
        public double twd { get; set; }
        public double uah { get; set; }
        public double usd { get; set; }
        public double vef { get; set; }
        public double vnd { get; set; }
        public double xag { get; set; }
        public double xau { get; set; }
        public double xdr { get; set; }
        public double zar { get; set; }
        public int bits { get; set; }
        public double link { get; set; }
        public int sats { get; set; }
    }

    public class DeveloperData
    {
        public int forks { get; set; }
        public int stars { get; set; }
        public int subscribers { get; set; }
        public int total_issues { get; set; }
        public int closed_issues { get; set; }
        public int pull_requests_merged { get; set; }
        public int pull_request_contributors { get; set; }
        public CodeAdditionsDeletions4Weeks code_additions_deletions_4_weeks { get; set; }
        public int commit_count_4_weeks { get; set; }
    }

    public class Image
    {
        public string thumb { get; set; }
        public string small { get; set; }
    }

    public class MarketCap
    {
        public double aed { get; set; }
        public double ars { get; set; }
        public double aud { get; set; }
        public double bch { get; set; }
        public double bdt { get; set; }
        public double bhd { get; set; }
        public double bmd { get; set; }
        public double brl { get; set; }
        public int btc { get; set; }
        public double cad { get; set; }
        public double chf { get; set; }
        public double clp { get; set; }
        public double cny { get; set; }
        public double czk { get; set; }
        public double dkk { get; set; }
        public double eth { get; set; }
        public double eur { get; set; }
        public double gbp { get; set; }
        public double hkd { get; set; }
        public double huf { get; set; }
        public long idr { get; set; }
        public double ils { get; set; }
        public double inr { get; set; }
        public double jpy { get; set; }
        public long krw { get; set; }
        public double kwd { get; set; }
        public double lkr { get; set; }
        public double ltc { get; set; }
        public double mmk { get; set; }
        public double mxn { get; set; }
        public double myr { get; set; }
        public double ngn { get; set; }
        public double nok { get; set; }
        public double nzd { get; set; }
        public double php { get; set; }
        public double pkr { get; set; }
        public double pln { get; set; }
        public double rub { get; set; }
        public double sar { get; set; }
        public double sek { get; set; }
        public double sgd { get; set; }
        public double thb { get; set; }
        public double @try { get; set; }
        public double twd { get; set; }
        public double uah { get; set; }
        public double usd { get; set; }
        public double vef { get; set; }
        public long vnd { get; set; }
        public double xag { get; set; }
        public double xau { get; set; }
        public double xdr { get; set; }
        public double zar { get; set; }
        public long bits { get; set; }
        public double link { get; set; }
        public long sats { get; set; }
    }

    public class MarketData
    {
        public CurrentPrice current_price { get; set; }
        public MarketCap market_cap { get; set; }
        public TotalVolume total_volume { get; set; }
    }

    public class PublicInterestStats
    {
        public int alexa_rank { get; set; }
        public object bing_matches { get; set; }
    }

    public class PassPriceDto
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public Image image { get; set; }
        public MarketData market_data { get; set; }
        public CommunityData community_data { get; set; }
        public DeveloperData developer_data { get; set; }
        public PublicInterestStats public_interest_stats { get; set; }
        public decimal GetPriceValueInUsd()
        {
            var usdPrice = (decimal)this.market_data.current_price.GetType()
                .GetProperties()
                .First(pr
                    => string.Equals(pr.Name, "usd", StringComparison.CurrentCultureIgnoreCase))
                .GetValue(this, null)!;

            return usdPrice;
        }
    }

    public class TotalVolume
    {
        public double aed { get; set; }
        public double ars { get; set; }
        public double aud { get; set; }
        public double bch { get; set; }
        public double bdt { get; set; }
        public double bhd { get; set; }
        public double bmd { get; set; }
        public double brl { get; set; }
        public double btc { get; set; }
        public double cad { get; set; }
        public double chf { get; set; }
        public double clp { get; set; }
        public double cny { get; set; }
        public double czk { get; set; }
        public double dkk { get; set; }
        public double eth { get; set; }
        public double eur { get; set; }
        public double gbp { get; set; }
        public double hkd { get; set; }
        public double huf { get; set; }
        public double idr { get; set; }
        public double ils { get; set; }
        public double inr { get; set; }
        public double jpy { get; set; }
        public double krw { get; set; }
        public double kwd { get; set; }
        public double lkr { get; set; }
        public double ltc { get; set; }
        public double mmk { get; set; }
        public double mxn { get; set; }
        public double myr { get; set; }
        public double ngn { get; set; }
        public double nok { get; set; }
        public double nzd { get; set; }
        public double php { get; set; }
        public double pkr { get; set; }
        public double pln { get; set; }
        public double rub { get; set; }
        public double sar { get; set; }
        public double sek { get; set; }
        public double sgd { get; set; }
        public double thb { get; set; }
        public double @try { get; set; }
        public double twd { get; set; }
        public double uah { get; set; }
        public double usd { get; set; }
        public double vef { get; set; }
        public double vnd { get; set; }
        public double xag { get; set; }
        public double xau { get; set; }
        public double xdr { get; set; }
        public double zar { get; set; }
        public double bits { get; set; }
        public double link { get; set; }
        public double sats { get; set; }
    }


}