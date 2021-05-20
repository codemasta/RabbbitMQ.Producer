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

        public static void DirectExchangePublish(IModel channel)
        {
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);

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
    }
}