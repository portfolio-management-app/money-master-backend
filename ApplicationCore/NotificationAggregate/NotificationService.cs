using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.PortfolioAggregate;


namespace ApplicationCore.NotificationAggregate
{
    public class NotificationService : IHostedService
    {
        private readonly ILogger<NotificationService> logger;
        private readonly IServiceScopeFactory scopeFactory;

        private Timer timer;

        public NotificationService(ILogger<NotificationService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Notification service is running !!");
            timer = new Timer(o =>
            {
                ReadDb();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Notification service has been stopped!!");
            return Task.CompletedTask;
        }

        public void ReadDb()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var portfolioService = scope.ServiceProvider.GetRequiredService<IPortfolioService>();
                var data = portfolioService.GetPortfolioList(1);
                data.ForEach((item) =>
                {
                    logger.LogInformation($"Portfolio name {item.Name}");
                });
            }

        }
    }
}
