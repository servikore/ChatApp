using Application.MessageHandlers;
using Application.MessageService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BotService
{
    public class BotServiceWorker : BackgroundService
    {
        private readonly ILogger<BotServiceWorker> _logger;
        private readonly IServiceBus serviceBus;
        private readonly IHttpClientFactory clientFactory;

        public BotServiceWorker(ILogger<BotServiceWorker> logger, IServiceBus serviceBus, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            this.serviceBus = serviceBus;
            this.clientFactory = clientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            serviceBus.SubscribeCommand<StockRequestCommand, StockRequestHandler>(new StockRequestHandler(clientFactory, serviceBus));
            
            _logger.LogInformation("BotServiceWorker has started.");

            await Task.Delay(Timeout.Infinite, stoppingToken);
            
            
        }
    }
}
