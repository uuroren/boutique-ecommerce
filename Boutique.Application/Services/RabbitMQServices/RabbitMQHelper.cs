using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.RabbitMQServices {
    public static class RabbitMQHelper {
        public static void SendMessage<T>(T message,string queueName) {
            var factory = new ConnectionFactory {
                HostName = "shrimp.rmq.cloudamqp.com",
                UserName = "rlutfbvs",
                Password = "AaXbpeJXLC97vwuc2rSM0rotOlrKURBm",
                VirtualHost = "rlutfbvs",
                Port = 5672
            };

            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: queueName,durable: true,exclusive: false,autoDelete: false,arguments: null);

                var json = JsonConvert.SerializeObject(message,new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "",routingKey: queueName,basicProperties: null,body: body);
            }
        }
    }
}
