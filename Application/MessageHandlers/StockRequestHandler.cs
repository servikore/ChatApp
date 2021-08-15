using Application.MessageService;
using Domain.Exceptions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.MessageHandlers
{
    public class StockRequestHandler : IntegrationCommandHandler<StockRequestCommand>
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IServiceBus serviceBus;

        public StockRequestHandler(IHttpClientFactory clientFactory, IServiceBus serviceBus) : base()
        {
            this.clientFactory = clientFactory;
            this.serviceBus = serviceBus;
        }
        public override async Task HandleMessage(StockRequestCommand message)
        {
            if (string.IsNullOrEmpty(message.StockCode))
                throw new InvalidSockCodeException();

            var stockData = await fetchStock(message.StockCode);
            var chatMessage = ParseStockData(stockData, message.StockCode);            
            serviceBus.PublishAsync(new StockProcessedEvent 
            {
                ChatMessage = chatMessage,
                FromUser = "@Bot",
                ToUser = message.User                
            });
            Console.WriteLine($"Command {stockData} received at {DateTime.Now}");            
        }

        private async Task<string> fetchStock(string StockCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://stooq.com/q/l/?s={StockCode}&f=sd2t2ohlcv&h&e=csv");
            request.Headers.Add("Accept", "text/plain");
            request.Headers.Add("User-Agent", "Stock_Bot");

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(responseMessage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockData">stockData</param>
        /// <param name="stockCode">StockCode</param>
        /// <returns>APPL.US quote is $CloseValue per share</returns>
        private string ParseStockData(string stockData, string stockCode)
        {
            if (string.IsNullOrEmpty(stockData))
                throw new InvalidSockCodeException { StockCode = stockCode };

            var lines = stockData.Split("\r\n");
            if(lines.Length < 2)
                throw new InvalidSockCodeException { StockCode = stockCode };
            
            var fields = lines[1].Split(",");
            if(fields.Length < 8)
                throw new InvalidSockCodeException { StockCode = stockCode };

            return $"{stockCode} quote is ${fields[6]} per share";
        }
    }
}
