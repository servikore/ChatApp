using System.Threading.Tasks;

namespace Application.MessageService
{
    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        string MessageKey { get; }
        Task HandleMessage(TMessage message);
    }

    public abstract class IntegrationCommandHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IntegrationCommand
    {
        public string MessageKey { get; private set; }
        public IntegrationCommandHandler()
        {
            MessageKey = ((System.Type[])((System.Reflection.TypeInfo)this.GetType()).ImplementedInterfaces)[0].GenericTypeArguments[0].Name;
        }
        public abstract Task HandleMessage(TMessage message);
    }

    public abstract class IntegrationEventHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IntegrationEvent
    {
        public string MessageKey { get; private set; }
        public IntegrationEventHandler()
        {
            MessageKey = ((System.Type[])((System.Reflection.TypeInfo)this.GetType()).ImplementedInterfaces)[0].GenericTypeArguments[0].Name;
        }
        public abstract Task HandleMessage(TMessage message);
        public abstract string Topic { get; }
    }
}
