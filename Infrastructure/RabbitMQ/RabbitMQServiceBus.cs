using Application.MessageService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RabbitMQ
{
    public class RabbitMQServiceBus : IServiceBus, IDisposable
    {
        private object thisLock = new object();
        private Dictionary<string, object> ttl;

        private readonly ConnectionFactory connectionFactory;
        private IConnection connection;
        public RabbitMQServiceBus(ILogger<RabbitMQServiceBus> logger, IOptions<RabbitMQSettings> options)
        {
            connectionFactory = new ConnectionFactory
            {
                UserName = options.Value.Username,
                Password = options.Value.Password,
                HostName = options.Value.Hostname,
                Port = options.Value.Port,
                VirtualHost = options.Value.VirtualHost
            };

            ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };
        }

        private void initConnection()
        {
            lock (thisLock)
            {
                connection = connectionFactory.CreateConnection();
            }
        }

        public void PublishAsync(IntegrationEvent eventMessage)
        {
            if (connection == null)
                initConnection();


            var channel = connection.CreateModel();

            channel.ExchangeDeclare(eventMessage.Topic, ExchangeType.Topic, durable: true, arguments: ttl);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventMessage));
            channel.BasicPublish(eventMessage.Topic, $"{eventMessage.Topic}.{eventMessage.MessageKey}", null, body);


        }

        public void SendAsync(IntegrationCommand command)
        {
            if (connection == null)
                initConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare(command.MessageKey, ExchangeType.Direct, durable: true, arguments: ttl);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
            channel.BasicPublish(command.MessageKey, command.MessageKey, null, body);
        }

        public void SubscribeCommand<T, TH>(TH commandHandler)
            where T : IntegrationCommand
            where TH : IntegrationCommandHandler<T>
        {
            if (connection == null)
                initConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare(commandHandler.MessageKey, ExchangeType.Direct, durable: true);
            channel.QueueDeclare(commandHandler.MessageKey,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(commandHandler.MessageKey, commandHandler.MessageKey, commandHandler.MessageKey);
            channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                commandHandler.HandleMessage(message);
            };

            channel.BasicConsume(commandHandler.MessageKey, true, consumer);

        }

        public void SubscribeEvent<T, TH>(TH eventHandler)
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>
        {
            if (connection == null)
                initConnection();

            var channel = connection.CreateModel();

            var queue = $"{eventHandler.Topic}-{eventHandler.MessageKey}";

            channel.ExchangeDeclare(eventHandler.Topic, ExchangeType.Topic, durable: true);
            channel.QueueDeclare(queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queue, eventHandler.Topic, $"{eventHandler.Topic}.*");
            channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                eventHandler.HandleMessage(message);              
            };

            channel.BasicConsume(queue, true, consumer);
        }

        public void Dispose()
        {
            connection?.Close();
            connection?.Dispose();
        }

    }
}
