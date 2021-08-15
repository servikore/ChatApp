using Application.MessageService;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using WebApp.Hubs;

namespace WebApp.MessageHandlers
{
    public class StockProcesssedHandler : IntegrationEventHandler<StockProcessedEvent>
    {
        private readonly IHubContext<ChatHub> hub;

        public override string Topic => "STOCK";
        public StockProcesssedHandler(IHubContext<ChatHub> hub)
        {
            this.hub = hub;
        }
        public override Task HandleMessage(StockProcessedEvent message)
        {
            hub.Clients.User(message.ToUser).SendAsync("ReceiveMessage",message.FromUser, message.ChatMessage);
            Console.WriteLine($"Event {message.ChatMessage} received at {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
