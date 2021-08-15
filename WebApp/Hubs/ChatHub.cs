using Application.DomainServices;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomService chatRoomService;

        public ChatHub(IChatRoomService chatRoomService)
        {
            this.chatRoomService = chatRoomService;
        }
        public async Task SendMessage(string user, string message)
        {
            var currentUser = Context.UserIdentifier;
            var handleResult = await chatRoomService.HandleMessage(int.Parse(currentUser), message);

            if (handleResult.isCommand && !handleResult.isValid)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "@bot", $"Invalid command {message}");
            }
            else if (!handleResult.isCommand && !handleResult.isValid)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "@bot", $"Invalid message");
            }
            else if (!handleResult.isCommand && handleResult.isValid)
            {
                await Clients.Others.SendAsync("ReceiveMessage", user, message);
            }         
        }
    }
}
