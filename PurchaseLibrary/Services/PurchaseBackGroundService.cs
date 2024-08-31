using CommonLibrary;
using CommonLibrary.DTOs;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PurchaseLibrary.Models;
using PurchaseLibrary.Services;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Text.Json;

namespace PurchaseAPI.Services
{
    public class PurchaseBackGroundService : BackgroundService
    {
        private string? exchange;
        private string? queue;
        private string? routingKey;
        private string? hostName;
        private int port;
        private IDistributedCache cache;
        private DistributedCacheEntryOptions cacheOptions;
        private IConfiguration configuration;
        private IServiceScopeFactory serviceScopeFactory;
        private readonly IConnection connection;
        private readonly IModel channel;

        public PurchaseBackGroundService(IConfiguration configuration,IDistributedCache cache,IServiceScopeFactory scopeFactory)
        {
            exchange = configuration.GetSection("RabbitMQ").GetSection("Exchange").Value;
            hostName = configuration.GetSection("RabbitMQ").GetSection("HostName").Value;
            port = Convert.ToInt16(configuration.GetSection("RabbitMQ").GetSection("Port").Value);
            this.cache = cache;
            cacheOptions = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(8000) };
            this.configuration = configuration;
            this.serviceScopeFactory = scopeFactory;

            var factory = new ConnectionFactory() { HostName = hostName, Port = port };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            channel.ExchangeDeclare(exchange: "MaterialExchange", type: ExchangeType.Topic);
            channel.ExchangeDeclare(exchange: "MaterialTypeExchange", type: ExchangeType.Topic);


            #region Material
              
            var materialQueue = channel.QueueDeclare().QueueName;
            var matConsumer = new EventingBasicConsumer(channel);

            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "AllRecords");
            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "Added");
            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "Updated");
            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "Deleted");


            matConsumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    BroadCastMessage broadCastMessage = JsonSerializer.Deserialize<BroadCastMessage>(message)!;

                    System.Console.WriteLine($"Material has changes..Refreshing cache..");

                    if (ea.RoutingKey == NotificationType.AllRecords.ToString())
                    {
                            if (broadCastMessage.Type == NotificationType.AllRecords)
                        {
                            var materials = JsonSerializer.Deserialize<List<Material>>(broadCastMessage.Message!);

                            using (var scope = serviceScopeFactory.CreateScope())
                            {
                                var myScopedService = scope.ServiceProvider.GetRequiredService<IMaterialService>();
                                await myScopedService.UpsertAllMaterialsAsync(materials!);
                            }
                            await cache.SetStringAsync("materialTypeMaster", message);
                        }
                    }
                    else if(ea.RoutingKey == NotificationType.Added.ToString())
                    {
                        var material = JsonSerializer.Deserialize<Material>(broadCastMessage.Message!);

                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IMaterialService>();
                            await myScopedService.UpsertMaterialAsync(material!);
                        }
                    }

                };

                channel.BasicConsume(queue: materialQueue, autoAck: true, consumer: matConsumer);
            #endregion

            #region MaterialType

            var materialTypeQueue = channel.QueueDeclare().QueueName;
            var matTypeConsumer = new EventingBasicConsumer(channel);

            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "AllRecords");
            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "Added");
            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "Updated");
            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "Deleted");


            matTypeConsumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                BroadCastMessage broadCastMessage = JsonSerializer.Deserialize<BroadCastMessage>(message)!;

                System.Console.WriteLine($"Material Type has changes..Refreshing cache..");

                if (broadCastMessage.Type == NotificationType.AllRecords)
                {
                    var materialTypes = JsonSerializer.Deserialize<List<MaterialType>>(broadCastMessage.Message!);

                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var myScopedService = scope.ServiceProvider.GetRequiredService<IMaterialService>();
                        await myScopedService.UpsertAllMaterialTypesAsync(materialTypes!);
                    }
                    await cache.SetStringAsync("materialTypeMaster", message);
                }

                if (broadCastMessage.Type == NotificationType.Added || broadCastMessage.Type == NotificationType.Updated || broadCastMessage.Type == NotificationType.Deleted)
                {
                    var materialType = JsonSerializer.Deserialize<MaterialType>(broadCastMessage.Message!);

                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var myScopedService = scope.ServiceProvider.GetRequiredService<IMaterialService>();
                        await myScopedService.UpsertMaterialTypeAsync(materialType!);
                    }
                    await cache.SetStringAsync("materialTypeMaster", message);
                }

            };

            channel.BasicConsume(queue: materialTypeQueue, autoAck: true, consumer: matTypeConsumer);
            #endregion

            await Task.Delay(Timeout.Infinite, stoppingToken);          
        }
    }
}
