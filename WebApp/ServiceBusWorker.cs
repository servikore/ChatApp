
using Application.MessageService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Hubs;
using WebApp.MessageHandlers;

namespace WebApp
{
    public class ServiceBusWorker : BackgroundService
    {
        private readonly ILogger<ServiceBusWorker> _logger;
        private readonly IServiceBus serviceBus;
        private readonly IHubContext<ChatHub> hub;

        public ServiceBusWorker(ILogger<ServiceBusWorker> logger, IServiceBus serviceBus, IHubContext<ChatHub> hub)
        {
            _logger = logger;
            this.serviceBus = serviceBus;
            this.hub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            serviceBus.SubscribeEvent<StockProcessedEvent, StockProcesssedHandler>(new StockProcesssedHandler(hub));

            _logger.LogInformation("ServiceBusWorker has started.");
            await Task.Delay(Timeout.Infinite, stoppingToken);
            
        }
    }
}
