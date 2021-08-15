using System.Threading.Tasks;

namespace Application.MessageService
{
    public interface IServiceBus
    {
        void PublishAsync(IntegrationEvent eventMessage);

        void SendAsync(IntegrationCommand command);

        void SubscribeCommand<T, TH>(TH commandHandler)
            where T : IntegrationCommand
            where TH : IntegrationCommandHandler<T>;           

        void SubscribeEvent<T,TH>(TH eventHandler)
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>;
    }
}
