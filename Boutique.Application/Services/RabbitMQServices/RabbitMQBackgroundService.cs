using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Boutique.Infrastructure.Repositories.ProductRepositories;
using Microsoft.Extensions.DependencyInjection;
using Boutique.Domain.Entities;

public class RabbitMQBackgroundService:BackgroundService {
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQBackgroundService(IServiceScopeFactory serviceScopeFactory) {
        _serviceScopeFactory = serviceScopeFactory;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ() {
        var factory = new ConnectionFactory {
            UserName = "rlutfbvs",
            Password = "AaXbpeJXLC97vwuc2rSM0rotOlrKURBm",
            Port = 5672,
            VirtualHost = "rlutfbvs",
            HostName = "shrimp.rmq.cloudamqp.com",
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "orderQueue",durable: true,exclusive: false,autoDelete: false,arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model,ea) => {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var order = JsonConvert.DeserializeObject<Order>(message);

            if(order != null) {
                using(var scope = _serviceScopeFactory.CreateScope()) {
                    var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    await UpdateProductStock(order,productRepository);
                }
            }

            _channel.BasicAck(deliveryTag: ea.DeliveryTag,multiple: false);
        };

        _channel.BasicConsume(queue: "orderQueue",autoAck: false,consumer: consumer);
        return Task.CompletedTask;
    }

    private async Task UpdateProductStock(Order order,IProductRepository productRepository) {
        foreach(var item in order.Items) {
            var product = await productRepository.GetProductByIdAsync(item.ProductId);
            if(product != null) {
                product.Stock -= item.Quantity;
                await productRepository.UpdateProductAsync(product);
            }
        }
    }

    public override void Dispose() {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
