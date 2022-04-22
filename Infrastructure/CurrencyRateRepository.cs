using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Interfaces;
using Mapster;

namespace Infrastructure
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        // cache using dictionary 
        private DateTime _lastCachedDay = DateTime.Now;
        private readonly Dictionary<string, CurrencyRate> _cachedCurrencyRates = new();

        public CurrencyRateRepository()
        {
            _lastCachedDay = DateTime.Now;
            _cachedCurrencyRates = new Dictionary<string, CurrencyRate>();
        }


        private class Rates
        {
            public decimal Usd { get; set; }
            public decimal Aed { get; set; }
            public decimal Afn { get; set; }
            public decimal All { get; set; }
            public decimal Amd { get; set; }
            public decimal Ang { get; set; }
            public decimal Aoa { get; set; }
            public decimal Ars { get; set; }
            public decimal Aud { get; set; }
            public decimal Awg { get; set; }
            public decimal Azn { get; set; }
            public decimal Bam { get; set; }
            public decimal Bbd { get; set; }
            public decimal Bdt { get; set; }
            public decimal Bgn { get; set; }
            public decimal Bhd { get; set; }
            public decimal Bif { get; set; }
            public decimal Bmd { get; set; }
            public decimal Bnd { get; set; }
            public decimal Bob { get; set; }
            public decimal Brl { get; set; }
            public decimal Bsd { get; set; }
            public decimal Btn { get; set; }
            public decimal Bwp { get; set; }
            public decimal Byn { get; set; }
            public decimal Bzd { get; set; }
            public decimal Cad { get; set; }
            public decimal Cdf { get; set; }
            public decimal Chf { get; set; }
            public decimal Clp { get; set; }
            public decimal Cny { get; set; }
            public decimal Cop { get; set; }
            public decimal Crc { get; set; }
            public decimal Cup { get; set; }
            public decimal Cve { get; set; }
            public decimal Czk { get; set; }
            public decimal Djf { get; set; }
            public decimal Dkk { get; set; }
            public decimal Dop { get; set; }
            public decimal Dzd { get; set; }
            public decimal Egp { get; set; }
            public decimal Ern { get; set; }
            public decimal Etb { get; set; }
            public decimal Eur { get; set; }
            public decimal Fjd { get; set; }
            public decimal Fkp { get; set; }
            public decimal Fok { get; set; }
            public decimal Gbp { get; set; }
            public decimal Gel { get; set; }
            public decimal Ggp { get; set; }
            public decimal Ghs { get; set; }
            public decimal Gip { get; set; }
            public decimal Gmd { get; set; }
            public decimal Gnf { get; set; }
            public decimal Gtq { get; set; }
            public decimal Gyd { get; set; }
            public decimal Hkd { get; set; }
            public decimal Hnl { get; set; }
            public decimal Hrk { get; set; }
            public decimal Htg { get; set; }
            public decimal Huf { get; set; }
            public decimal Idr { get; set; }
            public decimal Ils { get; set; }
            public decimal Imp { get; set; }
            public decimal Inr { get; set; }
            public decimal Iqd { get; set; }
            public decimal Irr { get; set; }
            public decimal Isk { get; set; }
            public decimal Jep { get; set; }
            public decimal Jmd { get; set; }
            public decimal Jod { get; set; }
            public decimal Jpy { get; set; }
            public decimal Kes { get; set; }
            public decimal Kgs { get; set; }
            public decimal Khr { get; set; }
            public decimal Kid { get; set; }
            public decimal Kmf { get; set; }
            public decimal Krw { get; set; }
            public decimal Kwd { get; set; }
            public decimal Kyd { get; set; }
            public decimal Kzt { get; set; }
            public decimal Lak { get; set; }
            public decimal Lbp { get; set; }
            public decimal Lkr { get; set; }
            public decimal Lrd { get; set; }
            public decimal Lsl { get; set; }
            public decimal Lyd { get; set; }
            public decimal Mad { get; set; }
            public decimal Mdl { get; set; }
            public decimal Mga { get; set; }
            public decimal Mkd { get; set; }
            public decimal Mmk { get; set; }
            public decimal Mnt { get; set; }
            public decimal Mop { get; set; }
            public decimal Mru { get; set; }
            public decimal Mur { get; set; }
            public decimal Mvr { get; set; }
            public decimal Mwk { get; set; }
            public decimal Mxn { get; set; }
            public decimal Myr { get; set; }
            public decimal Mzn { get; set; }
            public decimal Nad { get; set; }
            public decimal Ngn { get; set; }
            public decimal Nio { get; set; }
            public decimal Nok { get; set; }
            public decimal Npr { get; set; }
            public decimal Nzd { get; set; }
            public decimal Omr { get; set; }
            public decimal Pab { get; set; }
            public decimal Pen { get; set; }
            public decimal Pgk { get; set; }
            public decimal Php { get; set; }
            public decimal Pkr { get; set; }
            public decimal Pln { get; set; }
            public decimal Pyg { get; set; }
            public decimal Qar { get; set; }
            public decimal Ron { get; set; }
            public decimal Rsd { get; set; }
            public decimal Rub { get; set; }
            public decimal Rwf { get; set; }
            public decimal Sar { get; set; }
            public decimal Sbd { get; set; }
            public decimal Scr { get; set; }
            public decimal Sdg { get; set; }
            public decimal Sek { get; set; }
            public decimal Sgd { get; set; }
            public decimal Shp { get; set; }
            public decimal Sll { get; set; }
            public decimal Sos { get; set; }
            public decimal Srd { get; set; }
            public decimal Ssp { get; set; }
            public decimal Stn { get; set; }
            public decimal Syp { get; set; }
            public decimal Szl { get; set; }
            public decimal Thb { get; set; }
            public decimal Tjs { get; set; }
            public decimal Tmt { get; set; }
            public decimal Tnd { get; set; }
            public decimal Top { get; set; }
            public decimal Try { get; set; }
            public decimal Ttd { get; set; }
            public decimal Tvd { get; set; }
            public decimal Twd { get; set; }
            public decimal Tzs { get; set; }
            public decimal Uah { get; set; }
            public decimal Ugx { get; set; }
            public decimal Uyu { get; set; }
            public decimal Uzs { get; set; }
            public decimal Ves { get; set; }
            public decimal Vnd { get; set; }
            public decimal Vuv { get; set; }
            public decimal Wst { get; set; }
            public decimal Xaf { get; set; }
            public decimal Xcd { get; set; }
            public decimal Xdr { get; set; }
            public decimal Xof { get; set; }
            public decimal Xpf { get; set; }
            public decimal Yer { get; set; }
            public decimal Zar { get; set; }
            public decimal Zmw { get; set; }
            public decimal Zwl { get; set; }
        }

        private class CurrencyExchangeApiDto
        {
            public string Result { get; set; }
            public string Provider { get; set; }
            public string Documentation { get; set; }
            public string TermsOfUse { get; set; }
            public int TimeLastUpdateUnix { get; set; }
            public string TimeLastUpdateUtc { get; set; }
            public int TimeNextUpdateUnix { get; set; }
            public string TimeNextUpdateUtc { get; set; }
            public int TimeEolUnix { get; set; }
            public string BaseCode { get; set; }
            public Rates Rates { get; set; }
        }

        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl = "https://open.er-api.com/v6/latest/";

        public CurrencyRateRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CurrencyRate> GetRateObject(string sourceCurrency)
        {
            if (_lastCachedDay.Date == DateTime.Now.Date &&
                _cachedCurrencyRates.TryGetValue(sourceCurrency, out var rates)) return rates;

            // call api 
            if (_lastCachedDay.Date != DateTime.Now.Date)
                _cachedCurrencyRates.Clear();
            rates = await GetRatesObjectThroughApi(sourceCurrency);

            if (!_cachedCurrencyRates.ContainsKey(sourceCurrency))
                _cachedCurrencyRates.Add(sourceCurrency, rates);
            _lastCachedDay = DateTime.Now;
            return rates;
        }

        private async Task<CurrencyRate> GetRatesObjectThroughApi(string sourceCurrency)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl);

            var apiResult = await client.GetAsync(sourceCurrency);
            if (!apiResult.IsSuccessStatusCode)
                throw new ApplicationException("Error while calling the currency API");
            var apiResultString = await apiResult.Content.ReadAsStringAsync();
            var currencyApiDto = JsonSerializer.Deserialize<CurrencyExchangeApiDto>(apiResultString,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            if (currencyApiDto?.Result != "success")
                return null;

            var rates = currencyApiDto.Rates;
            return rates.Adapt<CurrencyRate>();
        }
    }
}