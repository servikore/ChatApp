using Application.MessageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.RabbitMQ
{
    public static class RabbitMQRegistration
    {
        public static void RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {            
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            services.AddSingleton<IServiceBus, RabbitMQServiceBus>();
        }
    }
}
