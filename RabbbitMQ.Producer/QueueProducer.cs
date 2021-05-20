using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using RabbitMQ.Client;

namespace RabbbitMQ.Producer
{
    public static class QueueProducer
    {
        public static void DefaultExchangePublish(IModel channel)
        {
            channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var count = 0;

            while (true) // simulating one producer multiple consumers
            {
                var message = new { Name = "Producer", Message = $"Hello! Count : {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish("", "demo-queue", null, body);
                count++;
                Thread.Sleep(1000);
            }
        }

        public static void DirectExchangePublish(IModel channel) // uses a routing key
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl",30000}
            };

            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct, arguments:ttl);

            var count = 0;

            while (true) // simulating one producer multiple consumers
            {
                var message = new { Name = "Producer", Message = $"Hello! Count : {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish("demo-direct-exchange", "account.init", null, body);
                count++;
                Thread.Sleep(1000);
            }
        }

        public static void TopicExchangePublish(IModel channel) // uses pattern to match the routing key
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl",30000}
            };

            channel.ExchangeDeclare("demo-topic-exchange", ExchangeType.Topic, arguments: ttl);

            var count = 0;

            while (true) // simulating one producer multiple consumers
            {
                var message = new { Name = "Producer", Message = $"Hello! Count : {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish("demo-topic-exchange", "account.init", null, body);
                count++;
                Thread.Sleep(1000);
            }
        }

        public static void HeaderExchangePublish(IModel channel) // exact matches 
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl",30000}
            };

            channel.ExchangeDeclare("demo-header-exchange", ExchangeType.Headers, arguments: ttl);

            var count = 0;

            while (true) // simulating one producer multiple consumers
            {
                var message = new { Name = "Producer", Message = $"Hello! Count : {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> {{"account", "new"}};

                channel.BasicPublish("demo-header-exchange", string.Empty, properties, body);
                count++;
                Thread.Sleep(1000);
            }
        }

        public static void FanOutExchangePublish(IModel channel) // exact matches 
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl",30000}
            };

            channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout, arguments: ttl);

            var count = 0;

            while (true) // simulating one producer multiple consumers
            {
                var message = new { Name = "Producer", Message = $"Hello! Count : {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish("demo-fanout-exchange", string.Empty, null, body);
                count++;
                Thread.Sleep(1000);
            }
        }
    }
}