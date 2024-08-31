using CommonLibrary;
using CommonLibrary.DTOs;

using Microsoft.Extensions.Configuration;

using RabbitMQ.Client;

using System.Text;
using System.Text.Json;

namespace MaterialLibrary.Services
{
    public class MaterialChangeNotification : IMaterialChangeNotification
    {
        private string? exchange;
        private string? hostName;
        private int port;
        readonly IConfiguration configuration;

        public MaterialChangeNotification(IConfiguration configuration)
        {
            this.configuration = configuration;
            hostName = configuration.GetSection("RabbitMQ").GetSection("HostName").Value;
            port = Convert.ToInt16(configuration.GetSection("RabbitMQ").GetSection("Port").Value);
        }

        public Task SendMaterialChangeNotification(string message,NotificationType notificationType)
        {
            BroadCastMessage broadCastMessage = new()
            {
                Message = message,
                Type = notificationType
            };

            var msg = JsonSerializer.Serialize(broadCastMessage);

            var factory = new ConnectionFactory() { HostName = hostName, Port = port };
            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "MaterialExchange", type: ExchangeType.Topic);
                channel.BasicPublish(exchange: "MaterialExchange", routingKey: notificationType.ToString(), mandatory: false, basicProperties: null, body: Encoding.UTF8.GetBytes(msg));
            }

            return Task.CompletedTask;
        }

        public Task SendMaterialTypeChangeNotification(string message,NotificationType notificationType)
        {
            BroadCastMessage broadCastMessage = new()
            {
                Message = message,
                Type = notificationType
            };

            var msg = JsonSerializer.Serialize(broadCastMessage);

            var factory = new ConnectionFactory() { HostName = hostName, Port = port };
            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "MaterialTypeExchange", type: ExchangeType.Topic);
                channel.BasicPublish(exchange: "MaterialTypeExchange", routingKey: notificationType.ToString(),mandatory:false, basicProperties: null, body: Encoding.UTF8.GetBytes(msg));
            }

            return Task.CompletedTask;
        }

    }
}
