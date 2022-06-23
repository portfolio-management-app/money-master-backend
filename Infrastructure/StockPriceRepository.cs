using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Interfaces;
using Mapster;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure
{
    public class StockPriceRepository : IStockPriceRepository
    {
        private class StockCandleApiDto
        {
            public List<decimal> c { get; set; } = new();
            public List<decimal> h { get; set; } = new();
            public List<decimal> l { get; set; } = new();
            public List<decimal> o { get; set; } = new();
            public string s { get; set; }
            public List<long> t { get; set; } = new();
            public List<long> v { get; set; } = new();
        }

        private class StockPriceApiDto
        {
            public decimal c { get; set; }
            public decimal d { get; set; }
            public decimal dp { get; set; }
            public decimal h { get; set; }
            public decimal l { get; set; }
            public decimal o { get; set; }
            public decimal pc { get; set; }
            public decimal t { get; set; }
        }

        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl = "https://finnhub.io/api/v1/quote";
        private readonly string _baseUrlPassPrice = "https://finnhub.io/api/v1/stock/candle";
        private readonly string _apiKey = "caq7egiad3iecj6acvhg";

        public StockPriceRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<StockPriceDto> GetPriceInUsd(string symbolCode)
        {
            // set up client 
            var client = _clientFactory.CreateClient();
            var query = new Dictionary<string, string>()
            {
                ["symbol"] = symbolCode,
                ["token"] = _apiKey
            };

            var uri = QueryHelpers.AddQueryString(_baseUrl, query);
            var apiResult = await client.GetAsync(uri);
            if (!apiResult.IsSuccessStatusCode)
                throw new ApplicationException($"Finnhub error: {await apiResult.Content.ReadAsStringAsync()}");

            var resultString = await apiResult.Content.ReadAsStringAsync();
            var stockPriceApiDto = JsonSerializer.Deserialize<StockPriceApiDto>(resultString);
            if (stockPriceApiDto?.c == 0)
                return null;
            TypeAdapterConfig<StockPriceApiDto, StockPriceDto>
                .NewConfig()
                .Map(dest => dest.CurrentPrice, src => src.c)
                .Map(dest => dest.Change, src => src.d)
                .Map(dest => dest.PercentChange, src => src.dp)
                .Map(dest => dest.HighPriceOfTheDay, src => src.h)
                .Map(dest => dest.LowPriceOfTheDay, src => src.l)
                .Map(dest => dest.OpenPriceOfTheDay, src => src.o)
                .Map(dest => dest.PreviousClosePrice, src => src.pc);

            var stockPriceResultDto = new StockPriceDto();
            stockPriceApiDto.Adapt(stockPriceResultDto);
            return stockPriceResultDto;
        }

        public async Task<decimal> GetPassPriceInUsd(string symbolCode, DateTime time)
        {
            var client = _clientFactory.CreateClient();
            var mockStartTime = time - TimeSpan.FromDays(7);
            var mockEndTime = time + TimeSpan.FromDays(7);
            var queries = new Dictionary<string, string>
            {
                ["symbol"] = symbolCode,
                ["resolution"] = "D",
                ["from"] = ((DateTimeOffset)mockStartTime).ToUnixTimeSeconds().ToString(),
                ["to"] = ((DateTimeOffset)mockEndTime).ToUnixTimeSeconds().ToString(),
                ["token"] = _apiKey
            };
            var uri = QueryHelpers.AddQueryString(_baseUrlPassPrice, queries);
            var apiResult = await client.GetAsync(uri);
            if (!apiResult.IsSuccessStatusCode)
                throw new ApplicationException($"Finnhub error: {await apiResult.Content.ReadAsStringAsync()}");
            var resultString = await apiResult.Content.ReadAsStringAsync();
            var stockCandlePriceDto = JsonSerializer.Deserialize<StockCandleApiDto>(resultString);
            if (stockCandlePriceDto is null)
                throw new ApplicationException("Failed to read past price in the Finhubb candle API");

            return GetPassStockPriceFromCandleHelper(stockCandlePriceDto, time);
        }

        private static decimal GetPassStockPriceFromCandleHelper(StockCandleApiDto stockCandlePriceDto, DateTime time)
        {
            var listOpenPrice = stockCandlePriceDto.o;
            if (listOpenPrice is null || listOpenPrice.Count == 0)
                throw new ApplicationException("Failed to read the past price from open price of candles");

            var unixOfInputTime = ((DateTimeOffset)time).ToUnixTimeSeconds();

            var listDateOfCandle = stockCandlePriceDto.t;
            var closestDateTime = listDateOfCandle.Aggregate(long.MaxValue, (closest, next) =>
                Math.Abs(next - unixOfInputTime) < Math.Abs(closest - unixOfInputTime) ? next : closest);
            var indexOfClosesDateTime = listDateOfCandle.Select((dt, index) => new { dt = dt, index = index })
                .First(d => d.dt == closestDateTime).index;
            var priceInUsd = listOpenPrice[indexOfClosesDateTime];
            if (priceInUsd is 0)
                throw new ApplicationException("Failed to read the past price from open price of candles");
            return priceInUsd;
        }
    }
}