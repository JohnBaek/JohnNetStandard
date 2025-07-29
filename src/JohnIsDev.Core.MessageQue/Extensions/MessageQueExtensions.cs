using JohnIsDev.Core.MessageQue.Implements;
using JohnIsDev.Core.MessageQue.Interfaces;
using JohnIsDev.Core.MessageQue.Models.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace JohnIsDev.Core.MessageQue.Extensions;

/// <summary>
/// Provides extension methods for the IServiceCollection interface to simplify the setup of services related to message queuing functionality.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds RabbitMQ configuration and services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to which RabbitMQ services will be added.</param>
    /// <param name="configuration">The configuration object containing RabbitMQ settings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<RabbitMqConfig>(provider =>
        {
            IConfiguration config = configuration.GetSection("MessageQue:RabbitMQ");
            return new RabbitMqConfig
            {
                HostName = config["HostName"] ?? "localhost",
                Port = int.Parse(config["Port"] ?? "5672"),
                UserName = config["UserName"] ?? "guest",
                Password = config["Password"] ?? "guest"
            };
        });
        
        // Add Connection
        services.AddSingleton<IConnection>(provider =>
        {
            var config = provider.GetRequiredService<RabbitMqConfig>();
            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password,
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });
        
        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
        return services;
    }
}