using JohnIsDev.Core.MessageQue.Implements;
using JohnIsDev.Core.MessageQue.Interfaces;
using JohnIsDev.Core.MessageQue.Models.Configs;
using MassTransit;
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
        services.AddSingleton<RabbitMqConfig>(_ =>
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
            RabbitMqConfig config = provider.GetRequiredService<RabbitMqConfig>();
            ConnectionFactory factory = new ConnectionFactory
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

    /// <summary>
    /// Adds RabbitMQ and MassTransit configuration and services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to which RabbitMQ and MassTransit services will be added.</param>
    /// <param name="configuration">The configuration object containing RabbitMQ settings.</param>
    /// <param name="registerConsumers">An action to register consumers in the MassTransit configuration.</param>
    /// <param name="configureEndpointsAction">An action to configure endpoints for the RabbitMQ bus factory.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRabbitMqMassTransit(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator> registerConsumers,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> configureEndpointsAction
        )
    {
        services.AddMassTransit(massTransit =>
        {
            registerConsumers.Invoke(massTransit);
            
            IConfiguration config = configuration.GetSection("MessageQue:RabbitMQ");
            string hostName = config["HostName"] ?? "localhost";
            ushort port = ushort.Parse(config["Port"] ?? "5672");
            string userName = config["UserName"] ?? "guest";
            string password = config["Password"] ?? "guest";
            
            massTransit.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostName, port, "/", hostConfigure =>
                {
                    hostConfigure.Username(username: userName);
                    hostConfigure.Password(password: password);
                });
                
                configureEndpointsAction.Invoke(context, cfg);
            });
        });
        return services;
    }
}