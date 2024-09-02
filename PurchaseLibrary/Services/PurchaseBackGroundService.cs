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
            #region Declare Exchanges

            channel.ExchangeDeclare(exchange: "MaterialExchange", type: ExchangeType.Topic);
            channel.ExchangeDeclare(exchange: "MaterialTypeExchange", type: ExchangeType.Topic);
            channel.ExchangeDeclare(exchange: "VendorExchange", type: ExchangeType.Topic);

            #endregion

            #region Declare Queues and RoutingKeys

            var materialQueue = channel.QueueDeclare().QueueName;
            var materialTypeQueue = channel.QueueDeclare().QueueName;
            var vendorQueue = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "AllRecords");
            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "Added");
            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "Updated");
            channel.QueueBind(queue: materialQueue, exchange: "MaterialExchange", routingKey: "Deleted");

            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "AllRecords");
            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "Added");
            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "Updated");
            channel.QueueBind(queue: materialTypeQueue, exchange: "MaterialTypeExchange", routingKey: "Deleted");

            channel.QueueBind(queue: vendorQueue, exchange: "VendorExchange", routingKey: "AllRecords");
            channel.QueueBind(queue: vendorQueue, exchange: "VendorExchange", routingKey: "Added");
            channel.QueueBind(queue: vendorQueue, exchange: "VendorExchange", routingKey: "Updated");
            channel.QueueBind(queue: vendorQueue, exchange: "VendorExchange", routingKey: "Deleted");

            #endregion

            #region Consumer

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                BroadCastMessage broadCastMessage = JsonSerializer.Deserialize<BroadCastMessage>(message)!;

                if (ea.ConsumerTag == materialQueue)
                {
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
                    else if (ea.RoutingKey == NotificationType.Added.ToString())
                    {
                        var material = JsonSerializer.Deserialize<Material>(broadCastMessage.Message!);

                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IMaterialService>();
                            await myScopedService.UpsertMaterialAsync(material!);
                        }
                    }
                }

                if (ea.ConsumerTag == materialTypeQueue)
                {
                    if (ea.RoutingKey == NotificationType.AllRecords.ToString())
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
                }

                if (ea.ConsumerTag == vendorQueue)
                {
                    if (ea.RoutingKey == NotificationType.AllRecords.ToString())
                    {
                        var vendors = JsonSerializer.Deserialize<List<Vendor>>(broadCastMessage.Message!);

                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IVendorService>();
                            await myScopedService.UpsertAllVendorsAsync(vendors!);
                        }
                        await cache.SetStringAsync("vendorMaster", message);
                    }

                    if (broadCastMessage.Type == NotificationType.Added || broadCastMessage.Type == NotificationType.Updated || broadCastMessage.Type == NotificationType.Deleted)
                    {
                        var vendor = JsonSerializer.Deserialize<Vendor>(broadCastMessage.Message!);

                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IVendorService>();
                            await myScopedService.UpsertVendorAsync(vendor!);
                        }
                        await cache.SetStringAsync("vendorMaster", message);
                    }
                }
            };

            channel.BasicConsume(queue: materialQueue, autoAck: true, consumer: consumer,consumerTag: materialQueue);
            channel.BasicConsume(queue: materialTypeQueue, autoAck: true, consumer: consumer, consumerTag: materialTypeQueue);
            channel.BasicConsume(queue: vendorQueue, autoAck: true, consumer: consumer, consumerTag: vendorQueue);
            #endregion

            await Task.Delay(Timeout.Infinite, stoppingToken);          
        }
    }
}
