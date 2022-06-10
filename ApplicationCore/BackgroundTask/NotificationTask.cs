using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ExternalService;
using ApplicationCore.NotificationAggregate;
using ApplicationCore.Entity;
using Microsoft.VisualBasic;
using ApplicationCore.UserAggregate;
using ApplicationCore.UserNotificationAggregate;

namespace ApplicationCore.BackgroundTask
{
    public class NotificationTask : IHostedService
    {
        private readonly ILogger<NotificationTask> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private readonly FirebaseAdminMessaging _fireBaseService = new FirebaseAdminMessaging();

        private readonly string assetReachHigh = "assetReachValueHigh";

        private readonly string assetReachLow = "assetReachValueLow";
        public NotificationTask(ILogger<NotificationTask> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
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
            TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Notification service has been stopped!!");
            return Task.CompletedTask;
        }

        private void RunService()
        {
            _logger.LogInformation("Notification service run");
            RunCoinService();
            RunStockService();
        }

        private async void RunCoinService()
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var highSubscribers = notificationService.GetActiveHighNotifications("crypto");
            var lowSubscribers = notificationService.GetActiveLowNotifications("crypto");
            var highQueries = GetCryptoQueryString(highSubscribers);
            var lowQueries = GetCryptoQueryString(lowSubscribers);
            var highPrices = await GetCryptoPrice(highQueries);
            var lowPrices = await GetCryptoPrice(lowQueries);
            PushHighCryptoNotification(highPrices, highSubscribers);
            PushLowCryptoNotification(lowPrices, lowSubscribers);
        }

        private async void RunStockService()
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var highSubscribers = notificationService.GetActiveHighNotifications("stock");
            var lowSubscribers = notificationService.GetActiveLowNotifications("stock");
            await PushHighStockNotification(highSubscribers);
            await PushLowStockNotification(lowSubscribers);
        }

        private string[] GetCryptoQueryString(List<Notification> subscribers)
        {

            var coinCodes = new Dictionary<string, int>();
            var currencyCodes = new Dictionary<string, int>();
            foreach (var item in subscribers)
            {
                if (!coinCodes.ContainsKey(item.CoinCode))
                {
                    coinCodes.Add(item.CoinCode, 0);
                }
                if (!currencyCodes.ContainsKey(item.Currency))
                {
                    currencyCodes.Add(item.Currency, 0);
                }

            }

            string coinCodeQuery = coinCodes.Aggregate("", (current, code) => current + $"{code.Key},");

            string currencyQuery = currencyCodes.Aggregate("", (current, currency) => current + $"{currency.Key},");

            var result = new string[] { coinCodeQuery, currencyQuery };
            return result;
        }

        private async Task<Dictionary<string, Dictionary<string, decimal>>> GetCryptoPrice(string[] queries)
        {
            using var scope = _scopeFactory.CreateScope();
            var cryptoRateService = scope.ServiceProvider.GetRequiredService<ICryptoRateRepository>();
            var result = await cryptoRateService.GetListCoinPrice(queries[0], queries[1]);
            return result;
        }

        private async Task<decimal> GetStockPrice(string stockSymbol)
        {
            using var scope = _scopeFactory.CreateScope();
            var stockRateService = scope.ServiceProvider.GetRequiredService<IStockPriceRepository>();
            var result = await stockRateService.GetPrice(stockSymbol);
            return result.CurrentPrice;
        }

        private void PushHighCryptoNotification(Dictionary<string, Dictionary<string, decimal>> priceObject, List<Notification> subscribers)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userNotificationService = scope.ServiceProvider.GetRequiredService<IUserNotificationService>();
            foreach (var subscriber in subscribers)
            {
                subscriber.Currency = subscriber.Currency.ToLower();
                if (priceObject.ContainsKey(subscriber.CoinCode))
                {
                    if (priceObject[subscriber.CoinCode].ContainsKey(subscriber.Currency))
                    {
                        var value = priceObject[subscriber.CoinCode][subscriber.Currency];
                        if (value > subscriber.HighThreadHoldAmount && subscriber.HighThreadHoldAmount != 0)
                        {

                            var tokens = userService.GetUserFcmCodeByUserId(subscriber.UserId);
                            _fireBaseService.SendMultiNotification(tokens, BuildDataForNotification(subscriber, assetReachHigh));
                            notificationService.TurnOffHighNotificationById(subscriber.Id);
                            userNotificationService.InsertNewNotification(subscriber, assetReachHigh);
                        }
                    }
                }
            }
        }

        private void PushLowCryptoNotification(Dictionary<string, Dictionary<string, decimal>> priceObject, List<Notification> subscribers)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userNotificationService = scope.ServiceProvider.GetRequiredService<IUserNotificationService>();
            foreach (var subscriber in subscribers)
            {
                subscriber.Currency = subscriber.Currency.ToLower();
                if (priceObject.ContainsKey(subscriber.CoinCode))
                {
                    if (priceObject[subscriber.CoinCode].ContainsKey(subscriber.Currency))
                    {
                        var value = priceObject[subscriber.CoinCode][subscriber.Currency];
                        if (value < subscriber.LowThreadHoldAmount && subscriber.LowThreadHoldAmount != 0)
                        {

                            var tokens = userService.GetUserFcmCodeByUserId(subscriber.UserId);
                            _fireBaseService.SendMultiNotification(tokens, BuildDataForNotification(subscriber, assetReachLow));
                            notificationService.TurnOffLowNotificationById(subscriber.Id);
                            userNotificationService.InsertNewNotification(subscriber, assetReachLow);
                        }
                    }
                }
            }
        }

        private async Task PushHighStockNotification(List<Notification> subscribers)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userNotificationService = scope.ServiceProvider.GetRequiredService<IUserNotificationService>();
            foreach (var subscriber in subscribers)
            {
                var price = await GetStockPrice(subscriber.StockCode);
                if (price > subscriber.HighThreadHoldAmount && subscriber.HighThreadHoldAmount != 0)
                {
                    var tokens = userService.GetUserFcmCodeByUserId(subscriber.UserId);
                    _fireBaseService.SendMultiNotification(tokens, BuildDataForNotification(subscriber, assetReachHigh));
                    userNotificationService.InsertNewNotification(subscriber, assetReachHigh);
                    notificationService.TurnOffHighNotificationById(subscriber.Id);
                }

            }
        }

        private async Task PushLowStockNotification(List<Notification> subscribers)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userNotificationService = scope.ServiceProvider.GetRequiredService<IUserNotificationService>();
            foreach (var subscriber in subscribers)
            {
                var price = await GetStockPrice(subscriber.StockCode);
                if (price < subscriber.LowThreadHoldAmount && subscriber.LowThreadHoldAmount != 0)
                {
                    var tokens = userService.GetUserFcmCodeByUserId(subscriber.UserId);
                    _fireBaseService.SendMultiNotification(tokens, BuildDataForNotification(subscriber, assetReachLow));
                    notificationService.TurnOffLowNotificationById(subscriber.Id);
                    userNotificationService.InsertNewNotification(subscriber, assetReachLow);
                }
            }
        }

        private Dictionary<string, string> BuildDataForNotification(Notification notification, string type)
        {
            return new Dictionary<string, string>(){
                {"title","Asset reach value"},
                {"body","body"},
                {"assetId",notification.AssetId.ToString()},
                {"portfolioId",notification.PortfolioId.ToString()},
                {"assetType",notification.AssetType},
                {"assetName",notification.AssetName},
                {"type",type},
                {"high",notification.HighThreadHoldAmount.ToString()},
                {"low",notification.LowThreadHoldAmount.ToString()},
                {"currency",notification.Currency}
            };
        }


    }
}