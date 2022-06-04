using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;


namespace ApplicationCore.NotificationAggregate
{
    public class NotificationService : IHostedService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private List<string> _subscribeCoinIds = new List<string>();

        private List<string> _subscribeCoinCurrencies = new List<string>();

        private string _subscribeStocks = null;

        private Timer _timer;

        public NotificationService(ILogger<NotificationService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            GetListSubscribe();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Notification service is running !!");
            _timer = new Timer(o =>
            {
                RunService();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Notification service has been stopped!!");
            return Task.CompletedTask;
        }

        public void RunService()
        {
            _logger.LogInformation("Notification service run");
            RunCoinService();
        }
        //Read subscribe coin and stock from DB
        public void GetListSubscribe()
        {
            this._subscribeCoinIds.Add("bitcoin");
            this._subscribeCoinIds.Add("tenet");
            this._subscribeCoinCurrencies.Add("usd");
            this._subscribeCoinCurrencies.Add("vnd");
            this._subscribeCoinCurrencies.Add("eur");
        }

        //Get subscribe coin List price
        public void RunCoinService()
        {

            var query = GetQueryString();
            using (var scope = _scopeFactory.CreateScope())
            {
                var coinRepository = scope.ServiceProvider.GetRequiredService<ICryptoRateRepository>();
                var result = coinRepository.GetListCoinPrice(query[0], query[1]);
            }
        }

        public string[] GetQueryString()
        {

            string coinQuery = "";
            string currencyQuery = "";
            for (var i = 0; i < _subscribeCoinIds.Count; i++)
            {
                coinQuery += $"{_subscribeCoinIds[i]},";
            }
            for (var i = 0; i < _subscribeCoinCurrencies.Count; i++)
            {
                currencyQuery += $"{_subscribeCoinCurrencies[i]},";
            }
            string[] result = { coinQuery, currencyQuery };
            return result;
        }

        //Get subscribe stock List price
        public void RunStockService()
        {

        }
        //Get Bank Asset Current Price
        public void GetBankAssetCurrentPrice()
        {

        }
    }
}
