using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ApplicationCore.NotificationAggregate{
    public class NotificationService : IHostedService
    {
       private readonly ILogger<NotificationService> logger;
        private Timer timer;
        private int number;
 
        public NotificationService(ILogger<NotificationService> logger)
        {
            this.logger = logger;
        }
 
        public void Dispose()
        {
            timer?.Dispose();
        }
 
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(o => {
                Interlocked.Increment(ref number);
                logger.LogInformation($"Printing the worker number {number}");
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
 
            return Task.CompletedTask;
        }
 
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
