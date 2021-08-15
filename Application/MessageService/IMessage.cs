
using System;

namespace Application.MessageService
{
    public interface IMessage 
    {
        string MessageKey { get; }
        Guid Id { get;}
        DateTime TimeStam { get;}
    }

    public abstract class Message : IMessage
    {
        public Guid Id { get; private set; }
        public DateTime TimeStam { get; private set; }
        public string MessageKey { get; private set; }
        public Message()
        {
            this.Id = Guid.NewGuid();
            this.TimeStam = DateTime.UtcNow;
            this.MessageKey = this.GetType().Name;
        }        
    }
}
