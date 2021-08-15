
namespace Application.MessageService
{
    public abstract class IntegrationEvent : Message
    {
        public abstract string Topic { get; }
    }
}
