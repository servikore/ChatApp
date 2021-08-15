using Application.MessageService;
using Application.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.DomainServices
{
    public interface IChatRoomService
    {
        Task<(bool isCommand, bool isValid)> HandleMessage(int userId, string chatMessage);
    }

    public class ChatRoomService : IChatRoomService
    {
        private const string stock_command = "stock";
        private readonly List<string> validCommands = new List<string>()
        {
            stock_command
        };
        private readonly IServiceBus serviceBus;
        private readonly IRepository<Domain.Entities.Message> messageRepository;

        public ChatRoomService(IServiceBus serviceBus, IRepository<Domain.Entities.Message> messageRepository)
        {
            this.serviceBus = serviceBus;
            this.messageRepository = messageRepository;
        }

        public async Task<(bool isCommand, bool isValid)> HandleMessage(int userId, string chatMessage)
        {
            (bool isCommand, bool isValid) result = new(false, false);

            if (string.IsNullOrEmpty(chatMessage))            
                return new(false, false);
            
            if (!chatMessage.StartsWith("/"))
            {
                result = new(false, true);
            }
            else if (chatMessage.Split("=").Length != 2)
            {
                result = new(false, true);
            }
            else if (chatMessage.Split("=").Length == 2)
            {
                var command = chatMessage.Split("=")[0].Remove(0, 1);                
                result = new(true, validCommands.Contains(command));
            }            

            if (result.isCommand && result.isValid)
            {
                var command = chatMessage.Split("=")[0].Remove(0, 1);
                var commandArgument = chatMessage.Split("=")[1];
                processCommand(command, commandArgument, userId, chatMessage);                
            }
            else if (!result.isCommand && result.isValid)
            {                
                var msg = new Domain.Entities.Message { UserId = userId, Content = chatMessage };
                await messageRepository.InsertAsync(msg);
            }                

            return result;
        }

        private void processCommand(string commandName, string commandArgument, int UserId, string chatMessage)
        {
            if (commandName == stock_command)
                serviceBus.SendAsync(new StockRequestCommand { StockCode = commandArgument, User = UserId.ToString() });
        }

    }
}
