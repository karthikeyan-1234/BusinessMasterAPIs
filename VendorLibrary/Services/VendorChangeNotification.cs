using CommonLibrary;
using CommonLibrary.DTOs;

using Microsoft.Extensions.Configuration;

using RabbitMQ.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VendorLibrary.Services
{
    public class VendorChangeNotification : IVendorChangeNotification
    {
        private string? exchange;
        private string? hostName;
        private int port;

        public VendorChangeNotification(IConfiguration configuration)
        {
            hostName = configuration.GetSection("RabbitMQ").GetSection("HostName").Value;
            port = Convert.ToInt16(configuration.GetSection("RabbitMQ").GetSection("Port").Value);
        }

        public Task SendVendorChangeNotification(string message, NotificationType notificationType)
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
                channel.ExchangeDeclare(exchange: "VendorExchange", type: ExchangeType.Topic);
                channel.BasicPublish(exchange: "VendorExchange", routingKey: notificationType.ToString(), mandatory: false, basicProperties: null, body: Encoding.UTF8.GetBytes(msg));
            }

            return Task.CompletedTask;
        }
    }
}
