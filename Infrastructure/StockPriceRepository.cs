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
    public class StockPriceRepository: IStockPriceRepository
    {
        private class StockPriceApiDto
        {
            public decimal C { get; set; }
            public decimal D { get; set; }
            public decimal Dp { get; set; }
            public decimal H { get; set; }
            public decimal L { get; set; }
            public decimal O { get; set; }
            public decimal Pc  { get; set; }
            public decimal T { get; set; }
            
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
            HttpClient client = _clientFactory.CreateClient();
            var query = new Dictionary<string, string>()
            {
                ["symbol"] = symbolCode,
                ["token"] = "c99q2t2ad3iaj0qou8mg"
            };

            var uri = QueryHelpers.AddQueryString(_baseUrl, query);
            var apiResult = await client.GetAsync(uri);
            if (!apiResult.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Finnhub error: {await apiResult.Content.ReadAsStringAsync()}");
            }

            var resultString = await apiResult.Content.ReadAsStringAsync();
            var stockPriceApiDto = JsonSerializer.Deserialize<StockPriceApiDto>(resultString);
            if (stockPriceApiDto?.C == 0)
                return null; 
            TypeAdapterConfig<StockPriceApiDto, StockPriceDto>
                    .NewConfig()
                    .Map(dest => dest.CurrentPrice, src => src.C)
                    .Map(dest => dest.Change, src => src.D)
                    .Map(dest => dest.PercentChange, src => src.Dp)
                    .Map(dest => dest.HighPriceOfTheDay, src => src.H)
                    .Map(dest => dest.LowPriceOfTheDay, src => src.L)
                    .Map(dest => dest.OpenPriceOfTheDay, src => src.O)
                    .Map(dest => dest.PreviousClosePrice, src => src.Pc);

            var stockPriceResultDto = new StockPriceDto();
            stockPriceApiDto.Adapt(stockPriceResultDto);
            return stockPriceResultDto; 

        }
    }
}