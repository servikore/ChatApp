using Application.MessageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MessageHandlers
{
    public class StockProcesssedHandler : IntegrationEventHandler<StockProcessedEvent>
    {
        public override string Topic => "STOCK";

        public override Task HandleMessage(StockProcessedEvent message)
        {
            Console.WriteLine($"Event {message.ChatMessage} received at {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
