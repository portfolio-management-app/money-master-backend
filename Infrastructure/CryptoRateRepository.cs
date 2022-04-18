using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class CryptoRateRepository: ICryptoRateRepository
    {


        private readonly IHttpClientFactory _factory;
        private string _baseUrl = "https://api.coingecko.com/api/v3/simple/price";

        public CryptoRateRepository(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<decimal> GetCurrentPriceInCurrency(string cryptoId, string currencyCode)
        {
            HttpClient client = _factory.CreateClient();

            var query = new Dictionary<string, string>()
            {
                ["ids"] = cryptoId,
                ["vs_currencies"] = currencyCode,
                ["include_24hr_change"] = "true"
            };
            var uri = QueryHelpers.AddQueryString(_baseUrl, query);
            var apiResult = await client.GetAsync(uri);
            if (!apiResult.IsSuccessStatusCode)
            {
                throw new ApplicationException("Cannot call the coingeckopi for crypto price");
            }

            var apiResultJson = await apiResult.Content.ReadAsStringAsync();
   
            var tokensFromResult = apiResultJson.Split(':', ',', '{','}');
            var price = decimal.Parse(tokensFromResult[4]);
            return price;
        }
    }
}