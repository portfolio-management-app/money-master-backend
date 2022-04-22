using System;
using System.Collections.Generic;
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

        public StockPriceRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<StockPriceDto> GetPrice(string symbolCode)
        {
            // set up client 
            var client = _clientFactory.CreateClient();
            var query = new Dictionary<string, string>()
            {
                ["symbol"] = symbolCode,
                ["token"] = "c99q2t2ad3iaj0qou8mg"
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
    }
}